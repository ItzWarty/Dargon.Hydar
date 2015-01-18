using Dargon.PortableObjects;

namespace Dargon.Hydar.Networking.PortableObjects {
   public class InboundEnvelopeImpl<TPayload> : InboundEnvelope<TPayload>, IPortableObject {
      private InboundEnvelopeHeader header;
      private TPayload message;

      public InboundEnvelopeImpl() { }

      public InboundEnvelopeImpl(InboundEnvelopeHeader header, TPayload message) {
         this.header = header;
         this.message = message;
      }

      public InboundEnvelopeHeader Header { get { return header; } }
      public TPayload Message { get { return message; } }
      object InboundEnvelope.Message { get { return message; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteObject(0, header);
         writer.WriteObject(1, message);
      }

      public void Deserialize(IPofReader reader) {
         header = (InboundEnvelopeHeader)reader.ReadObject(0);
         message = reader.ReadObject<TPayload>(1);
      }
   }
}