using Dargon.Hydar.PortableObjects;
using System;

namespace Dargon.Hydar.Networking.PortableObjects {
   public interface OutboundEnvelopeFactory {
      OutboundEnvelope<TMessage> CreateUnicastEnvelope<TMessage>(Guid recipient, TMessage message);
      OutboundEnvelope<TMessage> CreateBroadcastEnvelope<TMessage>(TMessage message);
   }

   public class OutboundEnvelopeFactoryImpl : OutboundEnvelopeFactory {
      private readonly HydarIdentity identity;

      public OutboundEnvelopeFactoryImpl(HydarIdentity identity) {
         this.identity = identity;
      }

      public OutboundEnvelope<TMessage> CreateUnicastEnvelope<TMessage>(Guid recipientId, TMessage message) {
         var header = new OutboundEnvelopeHeaderImpl(identity.NodeId, recipientId);
         return new OutboundEnvelopeImpl<TMessage>(header, message);
      }

      public OutboundEnvelope<TMessage> CreateBroadcastEnvelope<TMessage>(TMessage message) {
         var header = new OutboundEnvelopeHeaderImpl(identity.NodeId, Guid.Empty);
         return new OutboundEnvelopeImpl<TMessage>(header, message);
      }
   }
}
