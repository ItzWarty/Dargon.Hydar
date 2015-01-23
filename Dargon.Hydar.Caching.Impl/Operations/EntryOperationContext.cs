namespace Dargon.Hydar.Caching.Operations {
   public interface EntryOperationContext<K, V> {
      void EnqueueOperation(ManageableEntryOperation<K, V> operation);
      void HandleOperationComplete(ManageableEntryOperation<K, V> currentOperation);
   }
}
