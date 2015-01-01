namespace Dargon.Hydar.Caching {
   public interface CacheOperationContext<K, V> {
      CacheOperationStatus Status { get; }
      CacheOperationAccessFlags AccessFlags { get; }

      void HandleEnqueued();
      void HandleExecute(ManageableEntry<K, V> entryImpl);
      bool Abort();
   }

   public abstract class CacheOperationContextBase<K, V> : CacheOperationContext<K, V> {
      private readonly object synchronization = new object();
      private CacheOperationStatus status = CacheOperationStatus.Unattached;

      public CacheOperationStatus Status { get { return status; } }
      public abstract CacheOperationAccessFlags AccessFlags { get; }

      protected abstract void Execute(ManageableEntry<K, V> entry);

      public void HandleEnqueued() {
         lock (synchronization) {
            status = CacheOperationStatus.Pending;
         }
      }

      public void HandleExecute(ManageableEntry<K, V> entry) {
         lock (synchronization) {
            status = CacheOperationStatus.Running;
            Execute(entry);

            entry.ReleaseLock(this);
            status = CacheOperationStatus.Completed;
         }
      }

      public bool Abort() {
         lock (synchronization) {
            if (status == CacheOperationStatus.Unattached || status == CacheOperationStatus.Pending) {
               status = CacheOperationStatus.Aborted;
               return true;
            } else {
               return false;
            }
         }
      }
   }
}
