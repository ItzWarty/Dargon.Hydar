using System;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Proposals.Messages;

namespace Dargon.Hydar.Proposals.Phases {
   public class InitialPhase<K, V> : ProposalPhaseBase<K, V> {
      private readonly ProposalContext<K, V> proposalContext;
      private readonly ProposalPhaseFactory<K, V> proposalPhaseFactory;
      private readonly ActiveProposalManager<K, V> activeProposalManager;

      public InitialPhase(ProposalContext<K, V> proposalContext, ProposalPhaseFactory<K, V> proposalPhaseFactory, ActiveProposalManager<K, V> activeProposalManager) {
         this.proposalContext = proposalContext;
         this.proposalPhaseFactory = proposalPhaseFactory;
         this.activeProposalManager = activeProposalManager;
      }

      public override void Initialize() {
         base.Initialize();

         RegisterHandler<ProposalLeaderPrepare<K>>(HandleProposalPrepare);
      }

      public override bool TryBullyWith(ProposalContext<K, V> candidate) {
         throw new InvalidOperationException("Nonsensical to bully initial phase.");
      }

      private void HandleProposalPrepare(InboundEnvelopeHeader header, ProposalLeaderPrepare<K> message) {
         if (activeProposalManager.TryBully(message.EntryKey, proposalContext)) {
            var acceptedPhase = proposalPhaseFactory.AcceptedPhase(proposalContext);
            proposalContext.Transition(acceptedPhase);
         } else {
            var rejectedPhase = proposalPhaseFactory.RejectedPhase(proposalContext);
            proposalContext.Transition(rejectedPhase);
         }
      }
   }
}
