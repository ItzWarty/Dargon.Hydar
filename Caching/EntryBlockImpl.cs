using ItzWarty.Collections;

namespace Dargon.Hydar.Caching {
   public class EntryBlockImpl<K, V> : EntryBlock<K, V> {
      private readonly IConcurrentDictionary<K, ManageableEntry<K, V>> entriesByKey = new ConcurrentDictionary<K, ManageableEntry<K, V>>();
      private readonly int hashRangeOffset;
      private readonly int hashRangeSize;

      public EntryBlockImpl(int hashRangeOffset, int hashRangeSize) {
         this.hashRangeOffset = hashRangeOffset;
         this.hashRangeSize = hashRangeSize;
      }

      public int HashRangeOffset { get { return hashRangeOffset; } }
      public int HashRangeSize { get { return hashRangeSize; } }

      public ManageableEntry<K, V> GetEntry(K key) {
         return entriesByKey.GetOrAdd(key, k => new EntryImpl<K, V>(key));
      }
   }
}
