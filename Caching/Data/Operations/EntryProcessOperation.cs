namespace Dargon.Hydar.Caching.Data.Operations {
   public class EntryProcessOperation<K, V, R> : ReturningEntryOperation<K, V, R> {
      private readonly EntryProcessor<K, V, R> entryProcessor;

      public EntryProcessOperation(EntryProcessor<K, V, R> entryProcessor) {
         this.entryProcessor = entryProcessor;
      }

      public override EntryOperationType Type { get { return EntryOperationType.Update; } }

      public override R ExecuteInternal(ManageableEntry<K, V> entry) {
         return entryProcessor.Process(entry);
      }
   }
}
