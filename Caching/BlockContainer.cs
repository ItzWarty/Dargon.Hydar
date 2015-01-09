using System.Collections.Generic;
using Dargon.Hydar.Utilities;
using ItzWarty;

namespace Dargon.Hydar.Caching {
   public interface BlockContainer<K, V> {
      IReadOnlyList<EntryBlock<K, V>> Blocks { get; }

      EntryBlock<K, V> GetBlockForHash(int hash);
      ManageableEntry<K, V> GetEntryOrNull(K key); 
   }

   public class BlockContainerImpl<K, V> : BlockContainer<K, V> {
      private readonly HashSpacePartitioningStrategy partitioningStrategy;
      private readonly IReadOnlyList<EntryBlock<K, V>> blocks;  

      public BlockContainerImpl(HashSpacePartitioningStrategy partitioningStrategy) {
         this.partitioningStrategy = partitioningStrategy;
         this.blocks = Util.Generate(
            partitioningStrategy.BlockCount, 
            i => {
               var desc = partitioningStrategy.GetBlockDescriptor(i);
               return new EntryBlockImpl<K, V>(i, desc.Offset, desc.Length);
            }
         );
      }

      public IReadOnlyList<EntryBlock<K, V>> Blocks { get { return blocks; } }

      public EntryBlock<K, V> GetBlockById(int id) {
         return blocks[id];
      } 

      public EntryBlock<K, V> GetBlockForHash(int hash) {
         var mixedHash = HashUtilities.Mix(hash);
         return blocks[partitioningStrategy.GetBlockForHash(mixedHash)];
      }

      public ManageableEntry<K, V> GetEntryOrNull(K key) {
         return GetBlockForHash(key.GetHashCode()).GetEntry(key);
      }
   }
}
