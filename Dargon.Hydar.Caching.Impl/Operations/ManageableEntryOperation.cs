using System;

namespace Dargon.Hydar.Caching.Operations {
   public interface EntryOperation {
      EntryOperationStatus Status { get; }
      EntryAccessFlags AccessFlags { get; }

      bool Abort();
      void Wait();

      event EventHandler Completed;
      event EventHandler Aborted;
   }

   public interface ManageableEntryOperation<K, V> : EntryOperation {
      void HandleEnqueued();
      void HandleExecute(ManageableEntry<K, V> entry, EntryOperationContext<K, V> context);
   }
}
