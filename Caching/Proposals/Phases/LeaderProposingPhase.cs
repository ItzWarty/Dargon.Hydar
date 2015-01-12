using Dargon.Hydar.Caching.Proposals.Messages;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public class LeaderProposingPhase<K, V> : ProposalPhaseBase<K, V> {
      private readonly ProposalContext<K, V> proposalContext;
      private readonly ProposalPhaseFactory<K, V> proposalPhaseFactory;

      public LeaderProposingPhase(ProposalContext<K, V> proposalContext, ProposalPhaseFactory<K, V> proposalPhaseFactory) {
         this.proposalContext = proposalContext;
         this.proposalPhaseFactory = proposalPhaseFactory;
      }

      public override void Initialize() {
         base.Initialize();
      }

      public override void ProcessLeaderPrepare(InboundEnvelopeHeader header, ProposalLeaderPrepare<K> message, object activeProposalContextsByEntryKey) {
         if (message.ProposalId.CompareTo(proposalContext.Proposal.ProposalId) > 0) {
            Cancel();
         }
      }

      public override void ProcessFollowerAccept(InboundEnvelopeHeader header, ProposalFollowerAccept message) {
         throw new System.NotImplementedException();
      }

      public override void ProcessFollowerReject(InboundEnvelopeHeader header, ProposalFollowerReject message) {
         Cancel();
      }

      public override void ProcessLeaderCommit(InboundEnvelopeHeader header, ProposalLeaderCommit message) {
         throw new System.NotImplementedException();
      }

      public override void ProcessLeaderCancel(InboundEnvelopeHeader header, ProposalLeaderCancel message) {
         throw new System.NotImplementedException();
      }

      private void Commit() {

      }

      private void Cancel() {
         throw new System.NotImplementedException();
      }
   }
}