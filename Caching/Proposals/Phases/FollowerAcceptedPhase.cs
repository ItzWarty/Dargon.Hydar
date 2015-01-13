using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public class FollowerAcceptedPhase<K, V> : ProposalPhaseBase<K, V> {
      private ProposalContext<K, V> proposalContext;
      private ProposalPhaseFactoryImpl<K, V> proposalPhaseFactoryImpl;

      public FollowerAcceptedPhase(ProposalContext<K, V> proposalContext, ProposalPhaseFactoryImpl<K, V> proposalPhaseFactoryImpl) {
         this.proposalContext = proposalContext;
         this.proposalPhaseFactoryImpl = proposalPhaseFactoryImpl;
      }

      public override void Step() {
         base.Step();
      }

      public override bool TryBullyWith(ProposalContext<K, V> candidate) {
         return false;
      }
   }
}
