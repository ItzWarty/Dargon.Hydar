using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Proposals.Messages;

namespace Dargon.Hydar.Proposals.Phases {
   public class LeaderProposingPhase<K, V> : ProposalPhaseBase<K, V> {
      private readonly ProposalContext<K, V> proposalContext;
      private readonly ProposalPhaseFactory<K, V> proposalPhaseFactory;
      private readonly ActiveProposalManager<K, V> activeProposalManager;

      public LeaderProposingPhase(ProposalContext<K, V> proposalContext, ProposalPhaseFactory<K, V> proposalPhaseFactory, ActiveProposalManager<K, V> activeProposalManager) {
         this.proposalContext = proposalContext;
         this.proposalPhaseFactory = proposalPhaseFactory;
         this.activeProposalManager = activeProposalManager;
      }

      public override void Initialize() {
         base.Initialize();

         RegisterHandler<ProposalLeaderPrepare<K>>(HandleLeaderPrepare);
         RegisterHandler<ProposalFollowerAccept>(HandleFollowerAccept);
         RegisterHandler<ProposalFollowerReject>(HandleFollowerReject);
      }

      private void HandleLeaderPrepare(InboundEnvelopeHeader header, ProposalLeaderPrepare<K> message) {
         if (message.ProposalId.CompareTo(proposalContext.Proposal.ProposalId) > 0) {
            Cancel();
         }
      }

      private void HandleFollowerAccept(InboundEnvelopeHeader header, ProposalFollowerAccept message) {
         throw new System.NotImplementedException();
      }

      private void HandleFollowerReject(InboundEnvelopeHeader header, ProposalFollowerReject message) {
         Cancel();
      }

      public override bool TryBullyWith(ProposalContext<K, V> candidate) {
         if (candidate.Proposal.ProposalId.CompareTo(proposalContext.Proposal.ProposalId) > 0) {
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