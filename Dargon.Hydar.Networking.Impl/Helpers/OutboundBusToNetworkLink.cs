using Dargon.Audits;
using Dargon.Hydar.Networking.PortableObjects;

namespace Dargon.Hydar.Networking.Helpers {
   public class OutboundBusToNetworkLink {
      private readonly OutboundEnvelopeBus outboundEnvelopeBus;
      private readonly Network network;

      public OutboundBusToNetworkLink(OutboundEnvelopeBus outboundEnvelopeBus, Network network) {
         this.outboundEnvelopeBus = outboundEnvelopeBus;
         this.network = network;
      }

      public void Initialize() {
         outboundEnvelopeBus.EventPosted += HandleOutboundEnvelopeBusPosted;
      }

      internal void HandleOutboundEnvelopeBusPosted(EventBus<OutboundEnvelope> sender, OutboundEnvelope e) {
         network.Broadcast(e);
      }
   }
}
