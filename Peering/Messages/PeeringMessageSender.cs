using Dargon.Hydar.Networking;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Networking.Utilities;

namespace Dargon.Hydar.Peering.Messages {
   public interface PeeringMessageSender {
      void PeeringAnnounce();
   }

   public class PeeringMessageSenderImpl : MessageSenderBase, PeeringMessageSender {
      private readonly PeeringMessageFactory peeringMessageFactory;

      public PeeringMessageSenderImpl(
         OutboundEnvelopeFactory outboundEnvelopeFactory, 
         OutboundEnvelopeBus outboundEnvelopeBus, 
         PeeringMessageFactory peeringMessageFactory
      ) : base(outboundEnvelopeFactory, outboundEnvelopeBus) { 
         this.peeringMessageFactory = peeringMessageFactory;
      }

      public void PeeringAnnounce() {
         SendMessageBroadcast(peeringMessageFactory.PeeringAnnounce());
      }
   }
}
