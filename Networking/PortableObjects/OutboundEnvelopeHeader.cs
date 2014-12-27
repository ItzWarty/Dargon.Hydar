using Dargon.PortableObjects;
using System;

namespace Dargon.Hydar.PortableObjects {
   public interface OutboundEnvelopeHeader {
      Guid SenderGuid { get; }
      Guid RecipientGuid { get; }
   }

   public class OutboundEnvelopeHeaderImpl : OutboundEnvelopeHeader, IPortableObject {
      private Guid senderGuid;
      private Guid recipientGuid;

      public OutboundEnvelopeHeaderImpl() { }

      public OutboundEnvelopeHeaderImpl(Guid senderGuid, Guid recipientGuid) {
         this.senderGuid = senderGuid;
         this.recipientGuid = recipientGuid;
      }

      public Guid SenderGuid { get { return senderGuid; } }
      public Guid RecipientGuid { get { return recipientGuid; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, senderGuid);
         writer.WriteGuid(1, recipientGuid);
      }

      public void Deserialize(IPofReader reader) {
         senderGuid = reader.ReadGuid(0);
         recipientGuid = reader.ReadGuid(1);
      }
   }
}