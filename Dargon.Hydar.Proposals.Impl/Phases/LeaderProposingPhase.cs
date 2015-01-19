using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Proposals.Messages;

namespace Dargon.Hydar.Proposals.Phases {
   public class LeaderProposingPhase<K, V> : ProposalPhaseBase<K, V> {
      private readonly SubjectState<> subjectState;
      private readonly ProposalPhaseFactory<K, V> proposalPhaseFactory;
      private readonly ActiveProposalManager<K, V> activeProposalManager;

      public LeaderProposingPhase(SubjectState<> subjectState, ProposalPhaseFactory<K, V> proposalPhaseFactory, ActiveProposalManager<K, V> activeProposalManager) {
         this.subjectState = subjectState;
         this.proposalPhaseFactory = proposalPhaseFactory;
         this.activeProposalManager = activeProposalManager;
      }

      public override void Initialize() {
         base.Initialize();

         RegisterHandler<AtomicProposalPrepareImpl<K>>(HandleLeaderPrepare);
         RegisterHandler<AtomicProposalAcceptImpl>(HandleFollowerAccept);
         RegisterHandler<AtomicProposalRejectImpl>(HandleFollowerReject);
      }

      private void HandleLeaderPrepare(InboundEnvelopeHeader header, AtomicProposalPrepareImpl<K> message) {
         if (message.ProposalId.CompareTo(subjectState.AtomicProposal.ProposalId) > 0) {
            Cancel();
         }
      }

      private void HandleFollowerAccept(InboundEnvelopeHeader header, AtomicProposalAcceptImpl message) {
         throw new System.NotImplementedException();
      }

      private void HandleFollowerReject(InboundEnvelopeHeader header, AtomicProposalRejectImpl message) {
         Cancel();
      }

      public override bool TryBullyWith(SubjectState<> candidate) {
         if (candidate.AtomicProposal.ProposalId.CompareTo(subjectState.AtomicProposal.ProposalId) > 0) {
            Cancel();
            return true;
         }
         return false;
      }

      private void Commit() {

      }

      private void Cancel() {
         throw new System.NotImplementedException();
      }
   }
}