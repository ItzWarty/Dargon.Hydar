using System;
using Dargon.Hydar.Networking;
using Dargon.Hydar.Networking.PortableObjects;

namespace Dargon.Hydar.Utilities {
   public abstract class MessageSenderBase {
      protected readonly OutboundEnvelopeFactory outboundEnvelopeFactory;
      protected readonly OutboundEnvelopeBus outboundEnvelopeBus;

      protected MessageSenderBase(OutboundEnvelopeFactory outboundEnvelopeFactory, OutboundEnvelopeBus outboundEnvelopeBus) {
         this.outboundEnvelopeFactory = outboundEnvelopeFactory;
         this.outboundEnvelopeBus = outboundEnvelopeBus;
      }

      internal void SendMessageUnicast<TMessage>(Guid recipientId, TMessage message) {
         var envelope = outboundEnvelopeFactory.CreateUnicastEnvelope(recipientId, message);
         outboundEnvelopeBus.Post(envelope);
      }

      internal void SendMessageMulticast<TMessage>(Guid groupId, TMessage message) {
         SendMessageUnicast(groupId, message);
      }

      internal void SendMessageBroadcast<TMessage>(TMessage message) {
         var envelope = outboundEnvelopeFactory.CreateBroadcastEnvelope(message);
         outboundEnvelopeBus.Post(envelope);
      }
   }
}