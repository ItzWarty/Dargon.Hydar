using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty.Collections;
using System;
using Dargon.Audits;
using ItzWarty;

namespace Dargon.Hydar.Caching {
   public interface CacheRegistry {
      void RegisterCache(CacheContext cacheContext);
      CacheContext GetCacheOrNull(Guid cacheId);
   }

   public class CacheManagerImpl {
      private readonly HydarIdentity identity;
      private readonly InboundEnvelopeBus inboundEnvelopeBus;
      private readonly ConcurrentDictionary<Guid, CacheContext> cacheContextsById = new ConcurrentDictionary<Guid, CacheContext>();

      public CacheManagerImpl(HydarIdentity identity, InboundEnvelopeBus inboundEnvelopeBus) {
         this.identity = identity;
         this.inboundEnvelopeBus = inboundEnvelopeBus;
      }

      public void Initialize() {
         inboundEnvelopeBus.EventPosted += HandleInboundEnvelope;
      }

      public void RegisterCache(CacheContext cacheContext) {
         cacheContextsById.TryAdd(cacheContext.Id, cacheContext);
         identity.AddGroup(cacheContext.Id, cacheContext.Name);
      }

      public CacheContext GetCacheOrNull(Guid cacheId) {
         return cacheContextsById.GetValueOrDefault(cacheId);
      }

      private void HandleInboundEnvelope(EventBus<InboundEnvelope> sender, InboundEnvelope envelope) {
         CacheContext cacheContext;
         if (cacheContextsById.TryGetValue(envelope.Header.RecipientId, out cacheContext)) {
            cacheContext.Dispatch(envelope);
         }
      }

      // public void Tick() {
      //    foreach (var context in cacheContextsById.Values) {
      //       context.Tick();
      //    }
      // }
   }
}
