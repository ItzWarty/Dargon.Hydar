namespace Dargon.Hydar.Caching.Operations {
   public class EntryWriteOperation<K, V> : EntryOperationBase<K, V> {
      private readonly V value;

      public EntryWriteOperation(V value) {
         this.value = value;
      }

      public override EntryAccessFlags AccessFlags { get { return EntryAccessFlags.Write; } }

      protected override void Execute(ManageableEntry<K, V> entry) {
         entry.Value = value;
      }
   }
}
