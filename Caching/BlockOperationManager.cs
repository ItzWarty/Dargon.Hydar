using ItzWarty.Collections;

namespace Dargon.Hydar.Caching {
   public interface BlockOperationManager<K, V> {
      void EnqueueOperation(K key, EntryOperation<K, V> operation);
   }

   public class BlockOperationManagerImpl<K, V> : BlockOperationManager<K, V> {
      private readonly EntryBlock<K, V> block;
      private readonly ConcurrentDictionary<K, EntryOperationContext<K, V>> entryOperationContextsByKey = new ConcurrentDictionary<K, EntryOperationContext<K, V>>();

      public BlockOperationManagerImpl(EntryBlock<K, V> block) {
         this.block = block;
      }

      public void EnqueueOperation(K key, EntryOperation<K, V> operation) {
         var entryOperationContext = entryOperationContextsByKey.GetOrAdd(key, CreateEntryOperationContext);
         entryOperationContext.EnqueueOperation(operation);
      }

      private EntryOperationContext<K, V> CreateEntryOperationContext(K key) {
         return new EntryOperationContextImpl<K, V>(key, block);
      }
   }
}