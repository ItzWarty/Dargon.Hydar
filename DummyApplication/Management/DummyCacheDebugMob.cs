using Dargon.Hydar.Caching;
using Dargon.Hydar.Caching.Operations;
using Dargon.Management;
using DummyApplication.Hydar;

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

      [ManagedOperation]
      public string ToLower(int key) {
         var operation = new EntryProcessOperation<int, string, string>(new ToLowerProcessor());
         cacheOperationManager.EnqueueOperation(key, operation);
         var result = operation.GetResult();
         return key + ": " + (result ?? "null");
      }
   }
}
