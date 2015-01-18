using Dargon.Hydar.Caching.Data.Operations;
using Dargon.Hydar.Proposals;
using DummyApplication.Hydar;

namespace DummyApplication.Management {
   public class DummyCacheProposalMob {
      private readonly LocalProposalQueueManager<int, string> localProposalQueueManager;

      public DummyCacheProposalMob(LocalProposalQueueManager<int, string> localProposalQueueManager) {
         this.localProposalQueueManager = localProposalQueueManager;
      }

      public void ProposeToLower(int key) {
         localProposalQueueManager.EnqueueOperation(key, new EntryProcessOperation<int, string, string>(new ToLowerProcessor()));
      }
   }
}
