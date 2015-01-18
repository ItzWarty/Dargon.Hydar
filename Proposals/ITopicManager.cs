using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Proposals.Messages;

namespace Dargon.Hydar.Proposals {
   public interface ITopicManager {
      TopicDescriptor Descriptor { get; }

      FilterResult Filter(InboundEnvelope<IProposalMessage> envelope);
      PrepareResult Prepare(InboundEnvelope<ProposalLeaderPrepare> envelope);
      void Commit(InboundEnvelope<ProposalLeaderCommit> envelope);
      void Cancel(InboundEnvelope<ProposalLeaderCancel> envelope);
   }

   public enum FilterResult {
      Filter,
      Pass
   }

   public enum PrepareResult {
      Reject,
      Accept
   }
}
