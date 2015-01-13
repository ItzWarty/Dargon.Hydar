using Dargon.Hydar.Caching.Proposals.Messages;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public class LeaderProposingPhase<K, V> : ProposalPhaseBase {
      private readonly ProposalContext<K, V> proposalContext;
      private readonly ProposalPhaseFactory<K, V> proposalPhaseFactory;

      public LeaderProposingPhase(ProposalContext<K, V> proposalContext, ProposalPhaseFactory<K, V> proposalPhaseFactory) {
         this.proposalContext = proposalContext;
         this.proposalPhaseFactory = proposalPhaseFactory;
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

      private void Commit() {

      }

      private void Cancel() {
         throw new System.NotImplementedException();
      }
   }
}