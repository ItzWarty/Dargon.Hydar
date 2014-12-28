using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Caching {
   public interface CachingDispatcher {
   }

   public class CachingDispatcherImpl {
      private readonly InboundEnvelopeBus inboundEnvelopeBus;
      private readonly Guid registerCacheByGuid;

      public CachingDispatcherImpl(InboundEnvelopeBus inboundEnvelopeBus) {
         this.inboundEnvelopeBus = inboundEnvelopeBus;
      }

      public void Initialize() {
         inboundEnvelopeBus.EventPosted += HandleInboundEnvelope;
      }

      private void HandleInboundEnvelope(EventBus<InboundEnvelope> sender, InboundEnvelope e) { 

      }
   }
}
