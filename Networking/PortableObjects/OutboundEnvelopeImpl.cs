using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public class OutboundEnvelopeImpl<TMessage> : OutboundEnvelope<TMessage> {
      private OutboundEnvelopeHeader header;
      private TMessage message;

      public OutboundEnvelopeImpl() { } 

      public OutboundEnvelopeImpl(OutboundEnvelopeHeader header, TMessage message) {
         this.header = header;
         this.message = message;
      }

      public OutboundEnvelopeHeader Header { get { return header; } }
      public TMessage Message { get { return message; } }
      object OutboundEnvelope.Message { get { return Message; } }

      public InboundEnvelope<TMessage> ToInboundEnvelope(uint senderAddress) {
         var inboundHeader = new InboundEnvelopeHeaderImpl(senderAddress, header.SenderGuid, header.RecipientGuid);
         return new InboundEnvelopeImpl<TMessage>(inboundHeader, message);
      }

      InboundEnvelope OutboundEnvelope.ToInboundEnvelope(uint senderAddress) {
         return ToInboundEnvelope(senderAddress);
      }

      public void Serialize(IPofWriter writer) {
         writer.WriteObject(0, header);
         writer.WriteObject(1, message);
      }

      public void Deserialize(IPofReader reader) {
         header = reader.ReadObject<OutboundEnvelopeHeader>(0);
         message = reader.ReadObject<TMessage>(1);
      }
   }
}
