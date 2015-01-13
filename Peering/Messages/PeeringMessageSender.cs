using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Clustering.Messages;
using Dargon.Hydar.Networking;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Utilities;

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
