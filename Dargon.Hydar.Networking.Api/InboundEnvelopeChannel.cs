using ItzWarty.Threading;
using System;
using Dargon.Hydar.Networking.PortableObjects;

namespace Dargon.Hydar.Networking {
   public interface InboundEnvelopeChannel : IDisposable {
      void PostEnvelope(InboundEnvelope e);
      InboundEnvelope TakeEnvelope(ICancellationToken cancellationToken);
   }
}
