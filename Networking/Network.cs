using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Networking {
   public interface Network {
      void Broadcast(OutboundEnvelope envelope);

      event EnvelopeArrivedHandler EnvelopeArrived;
   }

   public delegate void EnvelopeArrivedHandler(Network sender, InboundEnvelope envelope);
}