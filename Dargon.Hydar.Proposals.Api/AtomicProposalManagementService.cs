using Dargon.Hydar.Proposals.Messages;

namespace Dargon.Hydar.Proposals {
   public interface AtomicProposalManagementService<TSubject> {
      void EnqueueProposal(TSubject key, Proposal<TSubject> proposal);
   }
}
