using System.Collections.Generic;
using Dargon.Hydar.Caching.Data.Partitioning;
using Dargon.Hydar.Caching.Operations;
using ItzWarty;

namespace Dargon.Hydar.Caching.Data {
   public interface CacheOperationManager<K, V> {
      void EnqueueOperation(K key, ManageableEntryOperation<K, V> operation);
   }

   public class CacheOperationManagerImpl<K, V> : CacheOperationManager<K, V> {
      private readonly HashSpacePartitioningStrategy partitioningStrategy;
      private readonly BlockContainer<K, V> blockContainer;
      private readonly IReadOnlyList<BlockOperationManager<K, V>> blockOperationManagers;

      public CacheOperationManagerImpl(HashSpacePartitioningStrategy partitioningStrategy, BlockContainer<K, V> blockContainer) {
         this.partitioningStrategy = partitioningStrategy;
         this.blockContainer = blockContainer;
         this.blockOperationManagers = Util.Generate(
            blockContainer.Blocks.Count,
            i => (BlockOperationManager<K, V>)new BlockOperationManagerImpl<K, V>(blockContainer.Blocks[i])
         );
      }

      public void EnqueueOperation(K key, ManageableEntryOperation<K, V> operation) {
         var blockId = partitioningStrategy.GetBlockForHash(key.GetHashCode());
         blockOperationManagers[blockId].EnqueueOperation(key, operation);
      }
   }
}
