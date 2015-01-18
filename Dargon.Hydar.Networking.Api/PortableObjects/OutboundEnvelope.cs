namespace Dargon.Hydar.Networking.PortableObjects {
   public interface OutboundEnvelope{
      OutboundEnvelopeHeader Header { get; }
      object Message { get; }
      InboundEnvelope ToInboundEnvelope(uint senderAddress);
   }

   public interface OutboundEnvelope<TMessage> : OutboundEnvelope {
      new TMessage Message { get; }
      new InboundEnvelope<TMessage> ToInboundEnvelope(uint senderAddress);
   }
}
