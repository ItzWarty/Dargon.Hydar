namespace Dargon.Hydar.Caching.Data.Operations {
   public class EntryWriteOperation<K, V> : EntryOperationBase<K, V> {
      private readonly V value;

      public EntryWriteOperation(V value) {
         this.value = value;
      }

      public override EntryOperationType Type { get { return EntryOperationType.Write; } }

      protected override void Execute(ManageableEntry<K, V> entry) {
         entry.Value = value;
      }
   }
}
