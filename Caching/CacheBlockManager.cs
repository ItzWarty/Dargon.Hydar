using System.Collections.Generic;

namespace Dargon.Hydar.Caching {
   public interface CacheBlockManager<K, V> {
      IReadOnlyList<EntryBlock<K, V>> Blocks { get; }

      EntryBlock<K, V> GetBlockOrNull(int hash);
      ManageableEntry<K, V> GetEntryOrNull(K key); 
   }
}
