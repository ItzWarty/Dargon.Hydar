namespace Dargon.Hydar.Caching.Operations {
   public class EntryWriteOperation<K, V> : EntryOperationBase<K, V> {
      private readonly V value;

      public EntryWriteOperation(V value) {
         this.value = value;
      }

      public override EntryOperationAccessFlags AccessFlags { get { return EntryOperationAccessFlags.Write; } }

      protected override void Execute(ManageableEntry<K, V> entry) {
         entry.Value = value;
      }
   }
}
