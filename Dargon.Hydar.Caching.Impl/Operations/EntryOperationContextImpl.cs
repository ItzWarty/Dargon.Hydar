using System;
using System.Collections.Generic;
using Dargon.Hydar.Caching.Data;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Caching.Operations {
   public class EntryOperationContextImpl<K, V> : EntryOperationContext<K, V> {
      private readonly K key;
      private readonly EntryBlock<K, V> block;
      private readonly object synchronization = new object();
      private readonly Queue<ManageableEntryOperation<K, V>> pendingOperations = new Queue<ManageableEntryOperation<K, V>>();
      private ManageableEntryOperation<K, V> activeOperation;
      private ManageableEntry<K, V> activeEntry;

      public EntryOperationContextImpl(K key, EntryBlock<K, V> block) {
         this.key = key;
         this.block = block;
      }

      public void EnqueueOperation(ManageableEntryOperation<K, V> operation) {
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

      public void HandleOperationComplete(ManageableEntryOperation<K, V> currentOperation) {
         using (var lockGuard = new LockGuard(synchronization)) {
            if (activeOperation != currentOperation) {
               throw new InvalidOperationException("Attempted to pass lock with inactive context.");
            }

            if (currentOperation.AccessFlags.HasFlag(EntryAccessFlags.Write)) {
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