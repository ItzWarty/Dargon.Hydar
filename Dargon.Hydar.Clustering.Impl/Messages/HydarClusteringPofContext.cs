using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Clustering.Messages.Helpers;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Clustering.Messages {
   public class HydarClusteringPofContext : PofContext {
      private const int kBasePofId = 2200;
      public HydarClusteringPofContext() {
         RegisterPortableObjectType(kBasePofId + 0, typeof(ElectionVote));
         RegisterPortableObjectType(kBasePofId + 1, typeof(ElectionAcknowledgement));
         RegisterPortableObjectType(kBasePofId + 2, typeof(EpochLeaderHeartBeat));
         RegisterPortableObjectType(kBasePofId + 3, typeof(ElectionCandidate));
         RegisterPortableObjectType(kBasePofId + 4, typeof(EpochSummaryImpl));
      }
   }
}
