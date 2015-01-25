using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Caching.Processors;
using Dargon.Hydar.Caching.Proposals;
using Dargon.Hydar.Proposals;

namespace Dargon.Hydar.Caching {
   public interface HydarCacheService<TKey, TValue> {
      void Put(TKey key, TValue value);
   }

   public class HydarCacheServiceImpl<TKey, TValue> : HydarCacheService<TKey, TValue> {
      private readonly AtomicProposalManagementService<TKey> atomicProposalManagementService;
      private readonly CacheProposalFactory<TKey, TValue> cacheProposalFactory;

      public HydarCacheServiceImpl(AtomicProposalManagementService<TKey> atomicProposalManagementService, CacheProposalFactory<TKey, TValue> cacheProposalFactory) {
         this.atomicProposalManagementService = atomicProposalManagementService;
         this.cacheProposalFactory = cacheProposalFactory;
      }

      public void Put(TKey key, TValue value) {
         var processor = new PutEntryProcessor<TKey, TValue>(value);
         var proposal = cacheProposalFactory.ProcessProposal(key, processor);
         atomicProposalManagementService.EnqueueProposal(key, proposal);
      }
   }
}
