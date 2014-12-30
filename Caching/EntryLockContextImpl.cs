using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Caching {
   public interface EntryLockContext<K, V> {
      void HandleEnqueued();
      void HandleLockAcquired(ManageableEntry<K, V> entryImpl);
      void HandleLockFreed();
   }

   public class EntryLockContextImpl<K, V> : EntryLockContext<K, V> {
      private readonly object synchronization = new object();
      private EntryLockStatus status = EntryLockStatus.Unattached;

      public void HandleEnqueued() {
         lock (synchronization) {
            status = EntryLockStatus.Pending;
         }
      }

      public void HandleLockAcquired(ManageableEntry<K, V> entry) {
         lock (synchronization) {
            status = EntryLockStatus.Acquired;
            entry.ReleaseLock(this);
         }
      }

      public void HandleLockFreed() {
         lock (synchronization) {
            status = EntryLockStatus.Released;
         }
      }
   }

   public enum EntryLockStatus {
      Unattached = 0,
      Pending = 1,
      Acquired = 2,
      Released = 3,
      Aborted = 4
   }
}
