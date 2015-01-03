﻿using System;

namespace Dargon.Hydar.Caching {
   public interface EntryOperation<K, V> {
      EntryOperationStatus Status { get; }
      EntryOperationAccessFlags AccessFlags { get; }

      void HandleEnqueued();
      void HandleExecute(ManageableEntry<K, V> entry, EntryOperationContext<K, V> context);
      bool Abort();

      event EventHandler Completed;
      event EventHandler Aborted;
   }

   public abstract class EntryOperationBase<K, V> : EntryOperation<K, V> {
      private readonly object synchronization = new object();
      private EntryOperationStatus status = EntryOperationStatus.Unattached;

      public event EventHandler Completed;
      public event EventHandler Aborted;

      public EntryOperationStatus Status { get { return status; } }
      public abstract EntryOperationAccessFlags AccessFlags { get; }

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
   }
}
