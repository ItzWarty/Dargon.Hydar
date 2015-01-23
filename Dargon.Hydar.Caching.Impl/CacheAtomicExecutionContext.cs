using Dargon.Hydar.Proposals;
using Dargon.Hydar.Proposals.Messages;

namespace Dargon.Hydar.Caching {
   public class CacheAtomicExecutionContext<TKey> : AtomicExecutionContext<TKey> {
      public void Execute(TKey subject, Proposal<TKey> proposal) {
         throw new System.NotImplementedException();
      }
   }
}
