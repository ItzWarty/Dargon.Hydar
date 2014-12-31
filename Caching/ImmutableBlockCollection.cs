using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItzWarty;

namespace Dargon.Hydar.Caching {
   // todo: is there an elegant immutable data structure with lower_bound functionality?
   public class ImmutableBlockCollection<K, V> {
      // separate keys from values to simpify code and reduce indirection
      private readonly int[] blockStarts;
      private readonly EntryBlock<K, V>[] blocks;

      public ImmutableBlockCollection(IReadOnlyList<EntryBlock<K, V>> blocks) {
         blockStarts = new int[blocks.Count];
         this.blocks = new EntryBlock<K, V>[blocks.Count];
         for (var i = 0; i < blocks.Count; i++) {
            var value = blocks[i];
            blockStarts[i] = value.HashRangeOffset;
            this.blocks[i] = value;
         }
         Array.Sort(blockStarts, this.blocks);
      }

      public int HashRangeOffset { get { return blockStarts[0]; } }

      public int HashRangeSize {
         get {
            var lastBlock = blocks[blocks.Length - 1];
            return lastBlock.HashRangeOffset + lastBlock.HashRangeSize - blockStarts[0];
         }
      }

      public IReadOnlyList<EntryBlock<K, V>> Values { get; }

      public EntryBlock<K, V> GetBlockOrNull(int hash) {
         var low = 0;
         var high = blockStarts.Length - 1;
         while (low < high) {
            var middle = low + (high - low) / 2;
            var middleBlockStart = blockStarts[middle];
            if (middleBlockStart < hash) {
               low = middle + 1;
            } else {
               high = middle - 1;
            }
         }
         if (low == 0 && blockStarts[low] > hash) {
            return null;
         } else if (low == blockStarts.Length - 1 && blockStarts[low] + blocks[low].HashRangeSize <= hash) {
            return null;
         } else if (blockStarts[low] > hash) {
            low--;
         }
         return blocks[low];
      } 
   }
}
