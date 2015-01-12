using Dargon.Hydar.Caching.Proposals.Messages;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public interface IProposalPhase<K, V> {
      void HandleEnter();
      void Step();
      void ProcessLeaderPrepare(InboundEnvelopeHeader header, ProposalLeaderPrepare<K> message, object activeProposalContextsByEntryKey);
      void ProcessFollowerAccept(InboundEnvelopeHeader header, ProposalFollowerAccept message);
      void ProcessFollowerReject(InboundEnvelopeHeader header, ProposalFollowerReject message);
      void ProcessLeaderCommit(InboundEnvelopeHeader header, ProposalLeaderCommit message);
      void ProcessLeaderCancel(InboundEnvelopeHeader header, ProposalLeaderCancel message);
   }
}