using Dargon.Hydar.Clustering.Messages;
using Dargon.Hydar.Utilities;
using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public class HydarPofContext : PofContext {
      private const int kBasePofId = 2000;

      public HydarPofContext() {
         // [0, 100) base stuff
         RegisterPortableObjectType(kBasePofId + 0, typeof(InboundEnvelopeImpl<>));
         RegisterPortableObjectType(kBasePofId + 1, typeof(InboundEnvelopeHeaderImpl));
         RegisterPortableObjectType(kBasePofId + 2, typeof(DateTimeInterval));

         // [100, 200) clustering suff
         RegisterPortableObjectType(kBasePofId + 100, typeof(ElectionVote));
         RegisterPortableObjectType(kBasePofId + 101, typeof(ElectionAcknowledgement));
         RegisterPortableObjectType(kBasePofId + 102, typeof(EpochLeaderHeartBeat));
      }
   }
}
