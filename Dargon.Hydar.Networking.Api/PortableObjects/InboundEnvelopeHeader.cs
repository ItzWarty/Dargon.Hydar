using System;

namespace Dargon.Hydar.Networking.PortableObjects {
   public interface InboundEnvelopeHeader {
      uint SenderAddress { get; }
      Guid SenderId { get; }
      Guid RecipientId { get; }
   }
}
