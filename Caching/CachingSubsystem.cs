using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty.Collections;
using System;

namespace Dargon.Hydar.Caching {
   public class CachingSubsystem : HydarSubsystem {
      private readonly ConcurrentDictionary<Guid, CacheContext> cacheContextsById = new ConcurrentDictionary<Guid, CacheContext>();

      public void AddCacheContext(CacheContext cacheContext) {
         cacheContextsById.TryAdd(cacheContext.Id, cacheContext);
      }

      public void Tick() {
         foreach (var context in cacheContextsById.Values) {
            context.Tick();
         }
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
            // We're not hosting the targeted cache.
            return true;
         }
         return cacheContext.Process(senderIdentity, (HydarMessage<CachingPayload>)message);
      }
   }
}
