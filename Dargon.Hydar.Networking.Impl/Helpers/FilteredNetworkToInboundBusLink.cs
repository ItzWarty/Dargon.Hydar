using System;
using Dargon.Hydar.Networking.PortableObjects;
using ItzWarty.Collections;
using Enumerable = System.Linq.Enumerable;

namespace Dargon.Hydar.Networking.Helpers {
   public class FilteredNetworkToInboundBusLink {
      private readonly Network network;
      private readonly InboundEnvelopeBus inboundMessageBus;
      private readonly HydarIdentity identity;

      public FilteredNetworkToInboundBusLink(Network network, InboundEnvelopeBus inboundMessageBus, HydarIdentity identity) {
         this.network = network;
         this.inboundMessageBus = inboundMessageBus;
         this.identity = identity;
      }

      public void Initialize() {
         network.EnvelopeArrived += HandleNetworkEnvelopeArrived;
      }

      internal void HandleNetworkEnvelopeArrived(Network sender, InboundEnvelope message) {
         var recipientId = message.Header.RecipientId;
         if (recipientId == Guid.Empty || recipientId == identity.NodeId || identity.GroupIds.Contains(recipientId)) {
            inboundMessageBus.Post(message);
         }
      }
   }
}
