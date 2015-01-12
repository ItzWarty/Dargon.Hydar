using Dargon.Hydar.Caching.Proposals.Messages;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public class InitialPhase<K, V> : ProposalPhaseBase<K, V> {
      private readonly ProposalContext<K, V> proposalContext;
      private readonly ProposalPhaseFactory<K, V> proposalPhaseFactory;

      public InitialPhase(ProposalContext<K, V> proposalContext, ProposalPhaseFactory<K, V> proposalPhaseFactory) {
         this.proposalContext = proposalContext;
         this.proposalPhaseFactory = proposalPhaseFactory;
      }

      public override void ProcessLeaderPrepare(InboundEnvelopeHeader header, ProposalLeaderPrepare<K> message, object activeProposalContextsByEntryKey) {
         var preparedPhase = proposalPhaseFactory.PreparedPhase(proposalContext);
         proposalContext.Transition(preparedPhase);
      }

      public override void ProcessFollowerAccept(InboundEnvelopeHeader header, ProposalFollowerAccept message) { }
      public override void ProcessFollowerReject(InboundEnvelopeHeader header, ProposalFollowerReject message) { }
      public override void ProcessLeaderCommit(InboundEnvelopeHeader header, ProposalLeaderCommit message) { }
      public override void ProcessLeaderCancel(InboundEnvelopeHeader header, ProposalLeaderCancel message) { }
   }
}
