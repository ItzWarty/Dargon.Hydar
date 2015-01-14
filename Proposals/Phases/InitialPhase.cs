using System;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Proposals.Messages;

namespace Dargon.Hydar.Proposals.Phases {
   public class InitialPhase<K, V> : ProposalPhaseBase<K, V> {
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

      public override bool TryBullyWith(ProposalContext<K, V> candidate) {
         throw new InvalidOperationException("Nonsensical to bully initial phase.");
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
