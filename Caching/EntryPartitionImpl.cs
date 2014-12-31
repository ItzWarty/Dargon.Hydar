using System.Collections.Generic;

namespace Dargon.Hydar.Caching {
   public class EntryPartitionImpl<K, V> {
      private readonly ImmutableBlockCollection<K, V> blocks;

      public EntryPartitionImpl(IReadOnlyList<EntryBlock<K, V>> blocks) {
         this.blocks = new ImmutableBlockCollection<K, V>(blocks);
      }

      public int HashRangeOffset { get { return blocks.HashRangeOffset; } }

      public int HashRangeSize { get { return blocks.HashRangeSize; } }

      public IReadOnlyList<EntryBlock<K, V>> Blocks {  get { return blocks.Values; } }

      public EntryBlock<K, V> GetBlockOrNull(int hash) {
         return blocks.GetBlockOrNull(hash);
      }
   }
}
