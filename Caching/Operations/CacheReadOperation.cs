namespace Dargon.Hydar.Caching.Operations {
   public class CacheReadOperation<K, V> : ReturningCacheOperation<K, V, V> {
      public override CacheOperationAccessFlags AccessFlags { get { return CacheOperationAccessFlags.Read; } }

      public override V ExecuteInternal(ManageableEntry<K, V> entry) {
         return entry.Value;
      }
   }
}
