using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItzWarty;

namespace Dargon.Hydar.Caching {
   public interface CacheOperationManager<K, V> {
      void EnqueueOperation(K key, EntryOperation<K, V> operation);
   }

   public class CacheOperationManagerImpl<K, V> : CacheOperationManager<K, V> {
      private readonly HashSpacePartitioningStrategy partitioningStrategy;
      private readonly CacheBlockContainer<K, V> blockContainer;
      private readonly IReadOnlyList<BlockOperationManager<K, V>> blockOperationManagers;

      public CacheOperationManagerImpl(HashSpacePartitioningStrategy partitioningStrategy, CacheBlockContainer<K, V> blockContainer) {
         this.partitioningStrategy = partitioningStrategy;
         this.blockContainer = blockContainer;
         this.blockOperationManagers = Util.Generate(
            blockContainer.Blocks.Count,
            i => (BlockOperationManager<K, V>)new BlockOperationManagerImpl<K, V>(blockContainer.Blocks[i])
         );
      }

      public void EnqueueOperation(K key, EntryOperation<K, V> operation) {
         var blockId = partitioningStrategy.GetBlockForHash(key.GetHashCode());
         blockOperationManagers[blockId].EnqueueOperation(key, operation);
      }
   }
}
