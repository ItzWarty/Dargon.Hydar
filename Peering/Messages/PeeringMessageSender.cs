using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Clustering.Messages;
using Dargon.Hydar.Networking;
using Dargon.Hydar.Networking.PortableObjects;

namespace Dargon.Hydar.Peering.Messages {
   public interface PeeringMessageSender {
      void PeeringAnnounce();
   }

   public class PeeringMessageSenderImpl : PeeringMessageSender {
      private readonly OutboundEnvelopeFactory outboundEnvelopeFactory;
      private readonly OutboundEnvelopeBus outboundEnvelopeBus;
      private readonly PeeringMessageFactory peeringMessageFactory;

      public PeeringMessageSenderImpl(OutboundEnvelopeFactory outboundEnvelopeFactory, OutboundEnvelopeBus outboundEnvelopeBus, PeeringMessageFactory peeringMessageFactory) {
         this.outboundEnvelopeFactory = outboundEnvelopeFactory;
         this.outboundEnvelopeBus = outboundEnvelopeBus;
         this.peeringMessageFactory = peeringMessageFactory;
      }

      public void PeeringAnnounce() {
         SendMessageBroadcast(peeringMessageFactory.PeeringAnnounce());
      }

      internal void SendMessageUnicast<TMessage>(Guid recipientId, TMessage message) {
         var envelope = outboundEnvelopeFactory.CreateUnicastEnvelope(recipientId, message);
         outboundEnvelopeBus.Post(envelope);
      }

      internal void SendMessageBroadcast<TMessage>(TMessage message) {
         var envelope = outboundEnvelopeFactory.CreateBroadcastEnvelope(message);
         outboundEnvelopeBus.Post(envelope);
      }
   }
}
