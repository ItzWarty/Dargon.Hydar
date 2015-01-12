using Dargon.Hydar.Caching.Proposals.Messages;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public class PreparedPhase<K, V> : ProposalPhaseBase<K, V> {
      private readonly ProposalContext<K, V> proposalContext;
      private readonly ProposalPhaseFactory<K, V> proposalPhaseFactory;

      public PreparedPhase(ProposalContext<K, V> proposalContext, ProposalPhaseFactory<K, V> proposalPhaseFactory) {
         this.proposalContext = proposalContext;
         this.proposalPhaseFactory = proposalPhaseFactory;
      }

      public override void ProcessLeaderPrepare(InboundEnvelopeHeader header, ProposalLeaderPrepare<K> message, object activeProposalContextsByEntryKey) {
         throw new System.NotImplementedException();
      }

      public override void ProcessFollowerAccept(InboundEnvelopeHeader header, ProposalFollowerAccept message) {
         throw new System.NotImplementedException();
      }

      public override void ProcessFollowerReject(InboundEnvelopeHeader header, ProposalFollowerReject message) {
         throw new System.NotImplementedException();
      }

      public override void ProcessLeaderCommit(InboundEnvelopeHeader header, ProposalLeaderCommit message) {
         throw new System.NotImplementedException();
      }

      public override void ProcessLeaderCancel(InboundEnvelopeHeader header, ProposalLeaderCancel message) {
         throw new System.NotImplementedException();
      }
   }
}