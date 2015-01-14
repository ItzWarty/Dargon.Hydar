using Dargon.Hydar.Caching.Data.Operations;
using Dargon.Hydar.Proposals;
using DummyApplication.Hydar;

namespace DummyApplication.Management {
   public class DummyCacheProposalMob {
      private readonly ProposalQueueManager<int, string> proposalQueueManager;

      public DummyCacheProposalMob(ProposalQueueManager<int, string> proposalQueueManager) {
         this.proposalQueueManager = proposalQueueManager;
      }

      public void ProposeToLower(int key) {
         proposalQueueManager.EnqueueOperation(key, new EntryProcessOperation<int, string, string>(new ToLowerProcessor()));
      }
   }
}
