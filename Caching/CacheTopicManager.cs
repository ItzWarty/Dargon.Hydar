//using Dargon.Hydar.Networking.PortableObjects;
//using Dargon.Hydar.Proposals;
//using Dargon.Hydar.Proposals.Messages;
//
//namespace Dargon.Hydar.Caching {
//   public class CacheTopicManager<K, V> : ITopicManager {
//      private readonly TopicDescriptor descriptor;
//
//      public CacheTopicManager(TopicDescriptor descriptor) {
//         this.descriptor = descriptor;
//      }
//
//      public TopicDescriptor Descriptor { get { return descriptor; } }
//
//      public FilterResult Filter(InboundEnvelope<IProposalMessage> proposal) {
//         if (!descriptor.Id.Equals(proposal.Message.TopicId)) {
//            return FilterResult.Filter;
//         }
//         return FilterResult.Pass;
//      }
//
//      public PrepareResult Prepare(InboundEnvelope<ProposalLeaderPrepare> key) {
//         return PrepareResult.Accept;
//      }
//
//      public void Commit(InboundEnvelope<ProposalLeaderCommit> envelope) {
//
//      }
//
//      public void Cancel(InboundEnvelope<ProposalLeaderCancel> envelope) {
//
//      }
//   }
//}
