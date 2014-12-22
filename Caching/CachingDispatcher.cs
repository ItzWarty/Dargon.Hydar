using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty.Collections;
using System;

namespace Dargon.Hydar.Caching {
   public class CachingDispatcher : HydarDispatcher {
      private readonly ConcurrentDictionary<Guid, CacheContext> cacheContextsById = new ConcurrentDictionary<Guid, CacheContext>();

      public void AddCacheContext(CacheContext cacheContext) {
         cacheContextsById.TryAdd(cacheContext.Id, cacheContext);
      }

      public bool Dispatch(IRemoteIdentity senderIdentity, HydarMessage message) {
         var payload = message.Payload as CachingPayload;
         if (payload == null) {
            return false;
         }

         var metadata = payload.Metadata;
         var cacheId = metadata.CacheId;
         CacheContext cacheContext;
         if (!cacheContextsById.TryGetValue(cacheId, out cacheContext)) {
            return false;
         }
         return cacheContext.Process(senderIdentity, (HydarMessage<CachingPayload>)message);
      }
   }
}
