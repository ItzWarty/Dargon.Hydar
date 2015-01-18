using System;

namespace Dargon.Hydar.Networking.PortableObjects {
   public interface OutboundEnvelopeHeader {
      Guid SenderGuid { get; }
      Guid RecipientGuid { get; }
   }
}