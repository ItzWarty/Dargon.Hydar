using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Caching {
   public interface ReadableEntry<K, V> {
      K Key { get; }
      V Value { get; }
      bool IsPresent { get; }
   }
   public interface ManageableEntry<K, V> : ReadableEntry<K, V> {
      new V Value { get; set; }

      void ReleaseLock(CacheOperationContext<K, V> currentLockContext);
   }

   public class EntryImpl<K, V> : ManageableEntry<K, V> {
      private readonly Queue<CacheOperationContext<K, V>> pendingLocks = new Queue<CacheOperationContext<K, V>>(); 
      private readonly object synchronization = new object();
      private readonly K key;
      private V value;
      private bool present;
      private CacheOperationContext<K, V> activeLock;

      public EntryImpl() { } 

      public EntryImpl(K key) : this(key, default(V), false) { }

      public EntryImpl(K key, V value, bool present) {
         this.key = key;
         this.value = value;
         this.present = present;
      }

      public K Key { get { return key; } }
      public V Value { get { return value; } set { this.value = value; } }
      public bool IsPresent { get { return present; } }

      public void EnqueueLock(CacheOperationContext<K, V> lockContext) {
         using (var lockGuard = new LockGuard(synchronization)) {
            if (activeLock == null) {
               activeLock = lockContext;
               lockGuard.Release();

               lockContext.HandleExecute(this);
            } else {
               pendingLocks.Enqueue(lockContext);
            }
         }
      }

      public void ReleaseLock(CacheOperationContext<K, V> currentLockContext) {
         using (var lockGuard = new LockGuard(synchronization)) {
            if (activeLock != currentLockContext) {
               throw new InvalidOperationException("Attempted to pass lock with inactive context.");
            }

            activeLock = null;
            
            if (pendingLocks.Count > 0) {
               activeLock = pendingLocks.Dequeue();
               lockGuard.Release();

               activeLock.HandleExecute(this);
            }
         }
      }
   }
}
