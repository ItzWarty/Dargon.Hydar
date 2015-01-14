using System;

namespace Dargon.Hydar.Proposals.Messages {
   public interface IProposalMessage {
      Guid CacheId { get; }
      Guid ProposalId { get; }
   }
}
