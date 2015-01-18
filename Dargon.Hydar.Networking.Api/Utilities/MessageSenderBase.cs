using System;
using Dargon.Hydar.Networking.PortableObjects;

namespace Dargon.Hydar.Networking.Utilities {
   public abstract class MessageSenderBase {
      protected readonly OutboundEnvelopeFactory outboundEnvelopeFactory;
      protected readonly OutboundEnvelopeBus outboundEnvelopeBus;

      protected MessageSenderBase(OutboundEnvelopeFactory outboundEnvelopeFactory, OutboundEnvelopeBus outboundEnvelopeBus) {
         this.outboundEnvelopeFactory = outboundEnvelopeFactory;
         this.outboundEnvelopeBus = outboundEnvelopeBus;
      }

      protected void SendMessageUnicast<TMessage>(Guid recipientId, TMessage message) {
         var envelope = outboundEnvelopeFactory.CreateUnicastEnvelope(recipientId, message);
         outboundEnvelopeBus.Post(envelope);
      }

      protected void SendMessageMulticast<TMessage>(Guid groupId, TMessage message) {
         SendMessageUnicast(groupId, message);
      }

      protected void SendMessageBroadcast<TMessage>(TMessage message) {
         var envelope = outboundEnvelopeFactory.CreateBroadcastEnvelope(message);
         outboundEnvelopeBus.Post(envelope);
      }
   }
}