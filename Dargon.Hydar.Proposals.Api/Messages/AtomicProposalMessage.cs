using System;

namespace Dargon.Hydar.Proposals.Messages {
   public interface AtomicProposalMessage {
      Guid ProposalId { get; }
   }
}
