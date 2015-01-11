namespace Dargon.Hydar.Caching.Proposals.Phases {
   public class PreparedPhase<K, V> : ProposalPhaseBase {
      private readonly ProposalContext<K, V> proposalContext;
      private readonly ProposalPhaseFactory<K, V> proposalPhaseFactory;

      public PreparedPhase(ProposalContext<K, V> proposalContext, ProposalPhaseFactory<K, V> proposalPhaseFactory) {
         this.proposalContext = proposalContext;
         this.proposalPhaseFactory = proposalPhaseFactory;
      }

      public override void Initialize() {
         base.Initialize();
      }
   }
}