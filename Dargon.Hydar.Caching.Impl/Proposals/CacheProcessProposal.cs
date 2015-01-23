using System;
using Dargon.Hydar.Caching.Processors;
using ItzWarty;
using ItzWarty.Collections;
using ItzWarty.Threading;

namespace Dargon.Hydar.Caching.Proposals {
   public interface CacheProcessProposal<TKey, TValue, TResult> : CacheProposal<TKey, TValue, TResult> {
      void Execute(ManageableEntry<TKey, TValue> entry);
      void WaitCompletion();
      TResult GetResult();
   }

   public class CacheProcessProposalImpl<TKey, TValue, TResult> : CacheProcessProposal<TKey, TValue, TResult> {
      private readonly IThreadingProxy threadingProxy;
      private readonly EntryProcessor<TKey, TValue, TResult> processor;
      private readonly ICountdownEvent completionLatch;
      private readonly object synchronization = new object();
      private TResult result;

      public CacheProcessProposalImpl(IThreadingProxy threadingProxy, EntryProcessor<TKey, TValue, TResult> processor) {
         this.threadingProxy = threadingProxy;
         this.processor = processor;
         this.completionLatch = threadingProxy.CreateCountdownEvent(1);
      }

      public TKey Subject { get { throw new NotImplementedException(); } }
      public IReadOnlySet<Guid> Participants { get { throw new NotImplementedException(); } }
      public Guid EpochId { get { throw new NotImplementedException(); } }

      public void Execute(ManageableEntry<TKey, TValue> entry) {
         result = processor.Process(entry);
         completionLatch.Signal();
      }

      public void WaitCompletion() {
         completionLatch.Wait();
      }

      public TResult GetResult() {
         WaitCompletion();
         return result;
      }
   }
}