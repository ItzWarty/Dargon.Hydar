using System;
using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public interface InboundEnvelope : IPortableObject {
      InboundEnvelopeHeader Header { get; }
      object Message { get; }
   }

   public interface InboundEnvelope<TMessage> : InboundEnvelope {
      new TMessage Message { get; }
   }
}
