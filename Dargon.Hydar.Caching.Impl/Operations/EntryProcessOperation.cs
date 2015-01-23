using Dargon.Hydar.Caching.Processors;

namespace Dargon.Hydar.Caching.Operations {
   public class EntryProcessOperation<K, V, R> : ReturningEntryOperation<K, V, R> {
      private readonly EntryProcessor<K, V, R> entryProcessor;

      public EntryProcessOperation(EntryProcessor<K, V, R> entryProcessor) {
         this.entryProcessor = entryProcessor;
      }

      public override EntryAccessFlags AccessFlags { get { return EntryAccessFlags.Write; } }

      public override R ExecuteInternal(ManageableEntry<K, V> entry) {
         return entryProcessor.Process(entry);
      }
   }
}
