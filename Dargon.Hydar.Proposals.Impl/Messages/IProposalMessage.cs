using System;

namespace Dargon.Hydar.Proposals.Messages {
   public interface IProposalMessage {
      Guid TopicId { get; }
      Guid ProposalId { get; }
   }
}
