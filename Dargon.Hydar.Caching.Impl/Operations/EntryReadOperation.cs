namespace Dargon.Hydar.Caching.Operations {
   public class EntryReadOperation<K, V> : ReturningEntryOperation<K, V, V> {
      public override EntryAccessFlags AccessFlags { get { return EntryAccessFlags.Read; } }

      public override V ExecuteInternal(ManageableEntry<K, V> entry) {
         return entry.Value;
      }
   }
}
