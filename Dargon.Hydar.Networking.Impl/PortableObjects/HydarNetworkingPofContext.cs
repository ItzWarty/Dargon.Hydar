using Dargon.PortableObjects;

namespace Dargon.Hydar.Networking.PortableObjects {
   public class HydarNetworkingPofContext : PofContext {
      private const int kBasePofId = 2100;

      public HydarNetworkingPofContext() {
         RegisterPortableObjectType(kBasePofId + 0, typeof(InboundEnvelopeImpl<>));
         RegisterPortableObjectType(kBasePofId + 1, typeof(InboundEnvelopeHeaderImpl));
         RegisterPortableObjectType(kBasePofId + 2, typeof(OutboundEnvelopeImpl<>));
         RegisterPortableObjectType(kBasePofId + 3, typeof(OutboundEnvelopeHeaderImpl));
      }
   }
}
