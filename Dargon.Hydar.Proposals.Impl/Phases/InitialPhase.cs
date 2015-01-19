using System;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Proposals.Messages;

namespace Dargon.Hydar.Proposals.Phases {
   public class InitialPhase<K, V> : ProposalPhaseBase<K, V> {
      private readonly SubjectState<> subjectState;
      private readonly ProposalPhaseFactory<K, V> proposalPhaseFactory;
      private readonly ActiveProposalManager<K, V> activeProposalManager;

      public InitialPhase(SubjectState<> subjectState, ProposalPhaseFactory<K, V> proposalPhaseFactory, ActiveProposalManager<K, V> activeProposalManager) {
         this.subjectState = subjectState;
         this.proposalPhaseFactory = proposalPhaseFactory;
         this.activeProposalManager = activeProposalManager;
      }

      public override void Initialize() {
         base.Initialize();

         RegisterHandler<AtomicProposalPrepareImpl<K>>(HandleProposalPrepare);
      }

      public override bool TryBullyWith(SubjectState<> candidate) {
         throw new InvalidOperationException("Nonsensical to bully initial phase.");
      }

      private void HandleProposalPrepare(InboundEnvelopeHeader header, AtomicProposalPrepareImpl<K> message) {
         if (activeProposalManager.TryBully(message.EntryKey, subjectState)) {
            var acceptedPhase = proposalPhaseFactory.AcceptedPhase(subjectState);
            subjectState.Transition(acceptedPhase);
         } else {
            var rejectedPhase = proposalPhaseFactory.RejectedPhase(subjectState);
            subjectState.Transition(rejectedPhase);
         }
      }
   }
}
