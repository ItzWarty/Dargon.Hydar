using System;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Networking.Helpers {
   public class FilteredNetworkToInboundBusLink {
      private readonly Network network;
      private readonly InboundEnvelopeBus inboundMessageBus;
      private readonly Guid localIdentifier;

      public FilteredNetworkToInboundBusLink(Network network, InboundEnvelopeBus inboundMessageBus, Guid localIdentifier) {
         this.network = network;
         this.inboundMessageBus = inboundMessageBus;
         this.localIdentifier = localIdentifier;
      }

      public void Initialize() {
         network.EnvelopeArrived += HandleNetworkEnvelopeArrived;
      }

      internal void HandleNetworkEnvelopeArrived(Network sender, InboundEnvelope message) {
         var recipientId = message.Header.RecipientId;
         if (recipientId == Guid.Empty || recipientId == localIdentifier) {
            inboundMessageBus.Post(message);
         }
      }
   }
}
