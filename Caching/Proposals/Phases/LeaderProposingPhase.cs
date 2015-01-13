using Dargon.Hydar.Caching.Proposals.Messages;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public class LeaderProposingPhase<K, V> : ProposalPhaseBase<K, V> {
      private readonly ProposalContext<K, V> proposalContext;
      private readonly ProposalPhaseFactory<K, V> proposalPhaseFactory;
      private readonly ActiveProposalRegistry<K, V> activeProposalRegistry;

      public LeaderProposingPhase(ProposalContext<K, V> proposalContext, ProposalPhaseFactory<K, V> proposalPhaseFactory, ActiveProposalRegistry<K, V> activeProposalRegistry) {
         this.proposalContext = proposalContext;
         this.proposalPhaseFactory = proposalPhaseFactory;
         this.activeProposalRegistry = activeProposalRegistry;
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