using Dargon.Hydar.PortableObjects;
using System;

namespace Dargon.Hydar.Networking.PortableObjects {
   public interface OutboundEnvelopeFactory {
      OutboundEnvelope<TMessage> CreateUnicastEnvelope<TMessage>(Guid recipient, TMessage message);
      OutboundEnvelope<TMessage> CreateBroadcastEnvelope<TMessage>(TMessage message);
   }

   public class OutboundEnvelopeFactoryImpl : OutboundEnvelopeFactory {
      private readonly Guid senderGuid;

      public OutboundEnvelopeFactoryImpl(Guid senderGuid) {
         this.senderGuid = senderGuid;
      }

      public OutboundEnvelope<TMessage> CreateUnicastEnvelope<TMessage>(Guid recipientId, TMessage message) {
         var header = new OutboundEnvelopeHeaderImpl(senderGuid, recipientId);
         return new OutboundEnvelopeImpl<TMessage>(header, message);
      }

      public OutboundEnvelope<TMessage> CreateBroadcastEnvelope<TMessage>(TMessage message) {
         var header = new OutboundEnvelopeHeaderImpl(senderGuid, Guid.Empty);
         return new OutboundEnvelopeImpl<TMessage>(header, message);
      }
   }
}
