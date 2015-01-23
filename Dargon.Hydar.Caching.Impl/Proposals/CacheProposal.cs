using System;
using Dargon.Hydar.Proposals;

namespace Dargon.Hydar.Caching.Proposals {
   public interface CacheProposal<TKey, TValue, TResult> : Proposal<TKey> {
      Guid EpochId { get; }
   }
}
