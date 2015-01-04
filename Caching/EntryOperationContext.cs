using Dargon.Hydar.Utilities;
using ItzWarty.Collections;
using System;

namespace Dargon.Hydar.Caching {
   public interface EntryOperationContext<K, V> {
      void EnqueueOperation(EntryOperation<K, V> operation);
      void HandleOperationComplete(EntryOperation<K, V> currentOperation);
   }

   public class EntryOperationContextImpl<K, V> : EntryOperationContext<K, V> {
      private readonly K key;
      private readonly EntryBlock<K, V> block;
      private readonly object synchronization = new object();
      private readonly Queue<EntryOperation<K, V>> pendingOperations = new Queue<EntryOperation<K, V>>();
      private EntryOperation<K, V> activeOperation;
      private ManageableEntry<K, V> activeEntry;

      public EntryOperationContextImpl(K key, EntryBlock<K, V> block) {
         this.key = key;
         this.block = block;
      }

      public void EnqueueOperation(EntryOperation<K, V> operation) {
         operation.HandleEnqueued();

         using (var lockGuard = new LockGuard(synchronization)) {
            if (activeOperation == null) {
               activeOperation = operation;
               lockGuard.Release();

               activeEntry = block.GetEntry(key);
               activeOperation.HandleExecute(activeEntry, this);
            } else {
               pendingOperations.Enqueue(operation);
            }
         }
      }

      public void HandleOperationComplete(EntryOperation<K, V> currentOperation) {
         using (var lockGuard = new LockGuard(synchronization)) {
            if (activeOperation != currentOperation) {
               throw new InvalidOperationException("Attempted to pass lock with inactive context.");
            }

            if (currentOperation.Type.HasFlag(EntryOperationType.Write)) {
               // todo: commit entry
            }

            activeEntry = null;
            activeOperation = null;

            if (pendingOperations.Count > 0) {
               activeOperation = pendingOperations.Dequeue();
               lockGuard.Release();

               activeOperation.HandleExecute(block.GetEntry(key), this);
            }
         }
      }
   }
}
