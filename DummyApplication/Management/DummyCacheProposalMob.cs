using Dargon.Hydar.Caching.Data.Operations;
using Dargon.Hydar.Proposals;
using DummyApplication.Hydar;

namespace DummyApplication.Management {
   public class DummyCacheProposalMob {
      private readonly LocalTopicProposalQueueManager<int, string> localTopicProposalQueueManager;

      public DummyCacheProposalMob(LocalTopicProposalQueueManager<int, string> localTopicProposalQueueManager) {
         this.localTopicProposalQueueManager = localTopicProposalQueueManager;
      }

      public void ProposeToLower(int key) {
         localTopicProposalQueueManager.EnqueueOperation(key, new EntryProcessOperation<int, string, string>(new ToLowerProcessor()));
      }
   }
}
