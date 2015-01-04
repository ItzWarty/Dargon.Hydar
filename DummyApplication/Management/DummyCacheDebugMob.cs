using Dargon.Hydar.Caching;
using Dargon.Hydar.Caching.Operations;
using Dargon.Management;

namespace DummyApplication.Management {
   public class DummyCacheDebugMob {
      private readonly CacheOperationManager<int, string> cacheOperationManager;

      public DummyCacheDebugMob(CacheOperationManager<int, string> cacheOperationManager) {
         this.cacheOperationManager = cacheOperationManager;
      }

      [ManagedOperation]
      public string Get(int key) {
         var readOperation = new EntryReadOperation<int, string>();
         cacheOperationManager.EnqueueOperation(key, readOperation);
         var result = readOperation.GetResult();
         return key + ": " + (result ?? "null");
      }

      [ManagedOperation]
      public string Put(int key, string value) {
         var operation = new EntryWriteOperation<int, string>(value);
         cacheOperationManager.EnqueueOperation(key, operation);
         operation.Wait();
         return "done";
      }
   }
}
