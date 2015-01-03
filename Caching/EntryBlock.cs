namespace Dargon.Hydar.Caching {
   public interface EntryBlock<K, V> {
      int HashRangeOffset { get; }
      int HashRangeSize { get; }

      ManageableEntry<K, V> GetEntry(K key);
   }
}