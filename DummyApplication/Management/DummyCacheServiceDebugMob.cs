using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Caching;
using Dargon.Management;

namespace DummyApplication.Management {
   public class DummyCacheServiceDebugMob<TKey, TValue> {
      private HydarCacheService<TKey, TValue> cacheService;

      public DummyCacheServiceDebugMob(HydarCacheService<TKey, TValue> cacheService) {
         this.cacheService = cacheService;
      }

      [ManagedOperation]
      public bool Put(TKey key, TValue value) {
         cacheService.Put(key, value);
         return true;
      }
   }
}
