namespace Dargon.Hydar.Caching.Operations {
   public class CacheWriteOperation<K, V> : CacheOperationContextBase<K, V> {
      private readonly V value;

      public CacheWriteOperation(V value) {
         this.value = value;
      }

      public override CacheOperationAccessFlags AccessFlags { get { return CacheOperationAccessFlags.Write; } }

      protected override void Execute(ManageableEntry<K, V> entry) {
         entry.Value = value;
      }
   }
}
