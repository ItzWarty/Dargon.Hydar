namespace Dargon.Hydar.Caching.Processors {
   public interface EntryProcessor<K, V, R> {
      EntryAccessFlags AccessFlags { get; }
      R Process(ManageableEntry<K, V> entry);
   }
}
