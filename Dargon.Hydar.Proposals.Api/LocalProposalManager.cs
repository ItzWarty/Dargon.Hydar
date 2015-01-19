using Dargon.Hydar.Proposals.Messages;

namespace Dargon.Hydar.Proposals {
   public interface LocalProposalManager<TSubject> {
      void EnqueueProposal(TSubject key, Proposal<TSubject> proposal);
   }
}
