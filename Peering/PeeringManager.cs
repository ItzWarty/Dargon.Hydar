using System;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Peering {
   public interface PeeringManager {
   }

   public class PeeringManagerImpl : EnvelopeProcessorBase<InboundEnvelope, Action<InboundEnvelope>>, PeeringManager {
      private readonly InboundEnvelopeBus inboundEnvelopeBus;

      public PeeringManagerImpl(InboundEnvelopeBus inboundEnvelopeBus) {
         this.inboundEnvelopeBus = inboundEnvelopeBus;
      }

      public void Initialize() {
         inboundEnvelopeBus.EventPosted += (s, e) => Process(e);

         RegisterHandler<>();
      }

      protected override void Invoke(Action<InboundEnvelope> handler, InboundEnvelope envelope) {
         handler(envelope);
      }
   }
}
