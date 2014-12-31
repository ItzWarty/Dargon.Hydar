using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Caching {
   public interface CacheOperationManager {
      
   }

   public class CacheOperationManagerImpl<K, V> : CacheOperationManager {
      private readonly CacheBlockManager<K, V> cacheBlockManager; 

      public CacheOperationManagerImpl(CacheBlockManager<K, V> cacheBlockManager) {
         this.cacheBlockManager = cacheBlockManager;
      }

      public ManageableEntry<K, V> GetEntry(K key) {
         return cacheBlockManager.GetEntryOrNull(key);
      }

      public V ReadEntry(K key) {
         return GetEntry(key).Value;
      }
   }
}
