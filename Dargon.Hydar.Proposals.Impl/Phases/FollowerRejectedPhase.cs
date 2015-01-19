using System;

namespace Dargon.Hydar.Proposals.Phases {
   public class FollowerRejectedPhase<K, V> : ProposalPhaseBase<K, V> {
      private readonly ProposalContext<K, V> proposalContext;

      public FollowerRejectedPhase(ProposalContext<K, V> proposalContext) {
         this.proposalContext = proposalContext;
      }

      public override void Initialize() {
         base.Initialize();
      }

      public override bool TryBullyWith(ProposalContext<K, V> candidate) {
         throw new InvalidOperationException("Nonsensical to bully a rejected context");
      }
   }
}
