using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Proposals.Messages;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Proposals {
   public class HydarAtomicProposalsPofContext : PofContext {
      private const int kBasePofId = 2600;

      public HydarAtomicProposalsPofContext() {
         RegisterPortableObjectType(kBasePofId + 0, typeof(AtomicProposalAcceptImpl));
         RegisterPortableObjectType(kBasePofId + 1, typeof(AtomicProposalCancelImpl));
         RegisterPortableObjectType(kBasePofId + 2, typeof(AtomicProposalCommitAcknowledgementImpl));
         RegisterPortableObjectType(kBasePofId + 3, typeof(AtomicProposalCommitImpl));
         RegisterPortableObjectType(kBasePofId + 4, typeof(AtomicProposalPrepareImpl<>));
         RegisterPortableObjectType(kBasePofId + 5, typeof(AtomicProposalRejectImpl));
      }
   }
}
