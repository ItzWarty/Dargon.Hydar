using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Caching.Processors;
using Dargon.Hydar.Caching.Proposals;
using Dargon.Hydar.Proposals;

namespace Dargon.Hydar.Caching {
   public interface HydarCacheProposingService<TKey, TValue> {
      TResult Process<TResult>(TKey key, EntryProcessor<TKey, TValue, TResult> processor);
   }

   public class HydarCacheProposingServiceImpl<TKey, TValue> : HydarCacheProposingService<TKey, TValue> {
      private readonly CacheProposalFactory<TKey, TValue> cacheProposalFactory;
      private readonly AtomicProposalManagementService<TKey> atomicProposalManagementService;

      public HydarCacheProposingServiceImpl(CacheProposalFactory<TKey, TValue> cacheProposalFactory, AtomicProposalManagementService<TKey> atomicProposalManagementService) {
         this.cacheProposalFactory = cacheProposalFactory;
         this.atomicProposalManagementService = atomicProposalManagementService;
      }

      public TResult Process<TResult>(TKey key, EntryProcessor<TKey, TValue, TResult> processor) {
         var proposal = cacheProposalFactory.ProcessProposal(key, processor);
         atomicProposalManagementService.EnqueueProposal(key, proposal);
         return proposal.GetResult();
      }
   }
}
