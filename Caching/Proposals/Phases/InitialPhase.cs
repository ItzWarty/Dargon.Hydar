using Dargon.Hydar.Caching.Proposals.Messages;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public class InitialPhase<K, V> : ProposalPhaseBase {
      private readonly ProposalContext<K, V> proposalContext;
      private readonly ProposalPhaseFactory<K, V> proposalPhaseFactory;

      public InitialPhase(ProposalContext<K, V> proposalContext, ProposalPhaseFactory<K, V> proposalPhaseFactory) {
         this.proposalContext = proposalContext;
         this.proposalPhaseFactory = proposalPhaseFactory;
      }

      public override void Initialize() {
         base.Initialize();

         RegisterHandler<ProposalLeaderPrepare<K>>(HandleProposalPrepare);
      }

      private void HandleProposalPrepare(InboundEnvelopeHeader header, ProposalLeaderPrepare<K> message) {
         var preparedPhase = proposalPhaseFactory.PreparedPhase(proposalContext);
         proposalContext.Transition(preparedPhase);
      }
   }
}
