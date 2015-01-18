namespace Dargon.Hydar.Networking.PortableObjects {
   public interface InboundEnvelope {
      InboundEnvelopeHeader Header { get; }
      object Message { get; }
   }

   public interface InboundEnvelope<out TMessage> : InboundEnvelope {
      new TMessage Message { get; }
   }
}
