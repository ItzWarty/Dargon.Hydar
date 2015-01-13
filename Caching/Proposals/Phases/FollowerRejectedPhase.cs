using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public class FollowerRejectedPhase<K, V> : ProposalPhaseBase {
      private readonly ProposalContext<K, V> proposalContext;

      public FollowerRejectedPhase(ProposalContext<K, V> proposalContext) {
         this.proposalContext = proposalContext;
      }

      public override void Initialize() {
         base.Initialize();
      }
   }
}
