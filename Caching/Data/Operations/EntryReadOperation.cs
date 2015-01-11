namespace Dargon.Hydar.Caching.Data.Operations {
   public class EntryReadOperation<K, V> : ReturningEntryOperation<K, V, V> {
      public override EntryOperationType Type { get { return EntryOperationType.Read; } }

      public override V ExecuteInternal(ManageableEntry<K, V> entry) {
         return entry.Value;
      }
   }
}
