using Dargon.Hydar.Caching.Proposals.Messages;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public class InitialPhase<K, V> : ProposalPhaseBase {
      private readonly ProposalContext<K, V> proposalContext;
      private readonly ProposalPhaseFactory<K, V> proposalPhaseFactory;
      private readonly ActiveProposalRegistry<K, V> activeProposalRegistry;

      public InitialPhase(ProposalContext<K, V> proposalContext, ProposalPhaseFactory<K, V> proposalPhaseFactory, ActiveProposalRegistry<K, V> activeProposalRegistry) {
         this.proposalContext = proposalContext;
         this.proposalPhaseFactory = proposalPhaseFactory;
         this.activeProposalRegistry = activeProposalRegistry;
      }

      public override void Initialize() {
         base.Initialize();

         RegisterHandler<ProposalLeaderPrepare<K>>(HandleProposalPrepare);
      }

      private void HandleProposalPrepare(InboundEnvelopeHeader header, ProposalLeaderPrepare<K> message) {
         if (activeProposalRegistry.TryBully(message.EntryKey, proposalContext)) {
            var acceptedPhase = proposalPhaseFactory.AcceptedPhase(proposalContext);
            proposalContext.Transition(acceptedPhase);
         } else {
            var rejectedPhase = proposalPhaseFactory.RejectedPhase(proposalContext);
            proposalContext.Transition(rejectedPhase);
         }
      }
   }
}
