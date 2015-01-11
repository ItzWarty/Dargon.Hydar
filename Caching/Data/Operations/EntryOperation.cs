using System;
using System.Threading;

namespace Dargon.Hydar.Caching.Data.Operations {
   public interface EntryOperation {
      EntryOperationStatus Status { get; }
      EntryOperationType Type { get; }

      bool Abort();
      void Wait();

      event EventHandler Completed;
      event EventHandler Aborted;
   }

   public interface EntryOperation<K, V> : EntryOperation {
      void HandleEnqueued();
      void HandleExecute(ManageableEntry<K, V> entry, EntryOperationContext<K, V> context);
   }

   public abstract class EntryOperationBase<K, V> : EntryOperation<K, V> {
      private readonly object synchronization = new object();
      private readonly CountdownEvent completionLatch = new CountdownEvent(1);
      private EntryOperationStatus status = EntryOperationStatus.Unattached;

      public event EventHandler Completed;
      public event EventHandler Aborted;

      public EntryOperationStatus Status { get { return status; } }
      public abstract EntryOperationType Type { get; }

      protected abstract void Execute(ManageableEntry<K, V> entry);

      public void HandleEnqueued() {
         lock (synchronization) {
            status = EntryOperationStatus.Pending;
         }
      }

      public void HandleExecute(ManageableEntry<K, V> entry, EntryOperationContext<K, V> context) {
         lock (synchronization) {
            if (status == EntryOperationStatus.Pending) {
               status = EntryOperationStatus.Running;
               Execute(entry);

               context.HandleOperationComplete(this);
               status = EntryOperationStatus.Completed;
               completionLatch.Signal();

               var capture = Completed;
               if (capture != null) {
                  capture(this, EventArgs.Empty);
               }
            }
         }
      }

      public bool Abort() {
         lock (synchronization) {
            if (status == EntryOperationStatus.Unattached || status == EntryOperationStatus.Pending) {
               status = EntryOperationStatus.Aborted;
               completionLatch.Signal();

               var capture = Aborted;
               if (capture != null) {
                  capture(this, EventArgs.Empty);
               }
               return true;
            } else {
               return false;
            }
         }
      }

      public void Wait() {
         completionLatch.Wait();
      }
   }
}
