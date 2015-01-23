using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Caching.Proposals;
using Dargon.Hydar.Proposals;

namespace Dargon.Hydar.Caching {
   public interface HydarCacheService<TKey, TValue> {
      
   }

   public class HydarCacheServiceImpl<TKey, TValue> : HydarCacheService<TKey, TValue> {
      private readonly AtomicProposalManagementService<TKey> atomicProposalManagementService;

      public HydarCacheServiceImpl(AtomicProposalManagementService<TKey> atomicProposalManagementService) {
         this.atomicProposalManagementService = atomicProposalManagementService;
      }

      public void Put(TKey key, TValue value) {
      }

//      public TResult Process<TResult>(TKey key, EntryProcessor<TKey, TValue, TResult> processor) {
//         atomicProposalManagementService.EnqueueProposal();
//         // atomicProposalManagementService.EnqueueProposal();
////         var state = subjectStateManager.GetOrCreate(key);
////         var processProposal = new CacheProcessProposal<TKey, TValue, TResult>(processor);
////         state.EnqueueProposal(processProposal);
//      }
   }
}
