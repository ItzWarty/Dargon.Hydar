using Dargon.Hydar.Caching;
using Dargon.Hydar.Caching.Data;
using Dargon.Hydar.Caching.Operations;
using Dargon.Hydar.Utilities;
using Dargon.Management;
using DummyApplication.Hydar;

namespace DummyApplication.Management {
   public class DummyCacheDebugMob {
      private readonly CacheOperationManager<int, string> cacheOperationManager;
      private readonly BlockContainer<int, string> blockContainer; 

      public DummyCacheDebugMob(CacheOperationManager<int, string> cacheOperationManager, BlockContainer<int, string> blockContainer) {
         this.cacheOperationManager = cacheOperationManager;
         this.blockContainer = blockContainer;
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

      [ManagedProperty]
      public int BlockCount { get { return blockContainer.Blocks.Count; } }

      [ManagedOperation]
      public int GetBlockIdForHash(int hash) {
         return blockContainer.GetBlockForHash(hash).Id;
      }

      [ManagedOperation]
      public int GetBlockIdForKey(int key) {
         return GetBlockIdForHash(key.GetHashCode());
      }

      [ManagedOperation]
      public int GetHashForKey(int key) {
         return HashUtilities.Mix(key.GetHashCode());
      }
   }
}
