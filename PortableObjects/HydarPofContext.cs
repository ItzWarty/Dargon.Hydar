using Dargon.Hydar.Caching.Operations;
using Dargon.Hydar.Clustering.Messages;
using Dargon.Hydar.Clustering.Messages.Helpers;
using Dargon.Hydar.Peering.Messages;
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
         RegisterPortableObjectType(kBasePofId + 3, typeof(OutboundEnvelopeImpl<>));
         RegisterPortableObjectType(kBasePofId + 4, typeof(OutboundEnvelopeHeaderImpl));

         // [100, 200) clustering suff
         RegisterPortableObjectType(kBasePofId + 100, typeof(ElectionVote));
         RegisterPortableObjectType(kBasePofId + 101, typeof(ElectionAcknowledgement));
         RegisterPortableObjectType(kBasePofId + 102, typeof(EpochLeaderHeartBeat));
         RegisterPortableObjectType(kBasePofId + 103, typeof(ElectionCandidate));
         RegisterPortableObjectType(kBasePofId + 104, typeof(EpochSummary));

         // [200, 300) peering stuff
         RegisterPortableObjectType(kBasePofId + 200, typeof(PeeringAnnounce));

         // [300, 400) caching stuff
         RegisterPortableObjectType(kBasePofId + 300, typeof(EntryReadOperation<,>));
         RegisterPortableObjectType(kBasePofId + 301, typeof(EntryWriteOperation<,>));
         RegisterPortableObjectType(kBasePofId + 302, typeof(EntryProcessOperation<,,>));
      }
   }
}
