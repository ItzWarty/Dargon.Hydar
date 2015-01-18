using System;

namespace Dargon.Hydar.Networking.PortableObjects {
   public interface OutboundEnvelopeFactory {
      OutboundEnvelope<TMessage> CreateUnicastEnvelope<TMessage>(Guid recipient, TMessage message);
      OutboundEnvelope<TMessage> CreateBroadcastEnvelope<TMessage>(TMessage message);
   }
}
