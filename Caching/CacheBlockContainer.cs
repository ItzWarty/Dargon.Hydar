using System.Collections.Generic;
using ItzWarty;

namespace Dargon.Hydar.Caching {
   public interface CacheBlockContainer<K, V> {
      IReadOnlyList<EntryBlock<K, V>> Blocks { get; }

      EntryBlock<K, V> GetBlockForHash(int hash);
      ManageableEntry<K, V> GetEntryOrNull(K key); 
   }

   public class CacheBlockContainerImpl<K, V> : CacheBlockContainer<K, V> {
      private readonly HashSpacePartitioningStrategy partitioningStrategy;
      private readonly IReadOnlyList<EntryBlock<K, V>> blocks;  

      public CacheBlockContainerImpl(HashSpacePartitioningStrategy partitioningStrategy) {
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
         return blocks[partitioningStrategy.GetBlockForHash(hash)];
      }

      public ManageableEntry<K, V> GetEntryOrNull(K key) {
         return GetBlockForHash(key.GetHashCode()).GetEntry(key);
      }
   }
}
