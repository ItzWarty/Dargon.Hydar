using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public interface OutboundEnvelope : IPortableObject {
      OutboundEnvelopeHeader Header { get; }
      object Message { get; }
      InboundEnvelope ToInboundEnvelope(uint senderAddress);
   }

   public interface OutboundEnvelope<TMessage> : OutboundEnvelope {
      new TMessage Message { get; }
      new InboundEnvelope<TMessage> ToInboundEnvelope(uint senderAddress);
   }
}
