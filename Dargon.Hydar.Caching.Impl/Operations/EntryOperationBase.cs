using System;
using System.Threading;

namespace Dargon.Hydar.Caching.Operations {
   public abstract class EntryOperationBase<K, V> : ManageableEntryOperation<K, V> {
      private readonly object synchronization = new object();
      private readonly CountdownEvent completionLatch = new CountdownEvent(1);
      private EntryOperationStatus status = EntryOperationStatus.Unattached;

      public event EventHandler Completed;
      public event EventHandler Aborted;

      public EntryOperationStatus Status { get { return status; } }
      public abstract EntryAccessFlags AccessFlags { get; }

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