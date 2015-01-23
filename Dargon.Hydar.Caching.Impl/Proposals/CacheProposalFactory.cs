using Dargon.Hydar.Caching.Processors;
using ItzWarty.Threading;

namespace Dargon.Hydar.Caching.Proposals {
   public interface CacheProposalFactory<TKey, TValue> {
      CacheProcessProposal<TKey, TValue, TResult> ProcessProposal<TResult>(EntryProcessor<TKey, TValue, TResult> processor);
   }

   public class CacheProposalFactoryImpl<TKey, TValue> : CacheProposalFactory<TKey, TValue> {
      private readonly IThreadingProxy threadingProxy;

      public CacheProposalFactoryImpl(IThreadingProxy threadingProxy) {
         this.threadingProxy = threadingProxy;
      }

      public CacheProcessProposal<TKey, TValue, TResult> ProcessProposal<TResult>(EntryProcessor<TKey, TValue, TResult> processor) {
         return new CacheProcessProposalImpl<TKey, TValue, TResult>(threadingProxy, processor);
      }
   }
}
