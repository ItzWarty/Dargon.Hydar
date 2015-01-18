using System;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Networking.PortableObjects {
   public class InboundEnvelopeHeaderImpl : InboundEnvelopeHeader, IPortableObject {
      private uint senderAddress;
      private Guid senderId;
      private Guid recipientId;

      public InboundEnvelopeHeaderImpl() { }

      public InboundEnvelopeHeaderImpl(uint senderAddress, Guid senderId, Guid recipientId) {
         this.senderAddress = senderAddress;
         this.senderId = senderId;
         this.recipientId = recipientId;
      }

      public uint SenderAddress { get { return senderAddress; } }
      public Guid SenderId { get { return senderId; } }
      public Guid RecipientId { get { return recipientId; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteU32(0, senderAddress);
         writer.WriteGuid(1, senderId);
         writer.WriteGuid(2, recipientId);
      }

      public void Deserialize(IPofReader reader) {
         senderAddress = reader.ReadU32(0);
         senderId = reader.ReadGuid(1);
         recipientId = reader.ReadGuid(2);
      }
   }
}