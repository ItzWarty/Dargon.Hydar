namespace Dargon.Hydar.Proposals {
   public class AtomicProposalManagementServiceImpl<TSubject> : AtomicProposalManagementService<TSubject> {
      private readonly SubjectStateManager<TSubject> subjectStateManager;

      public AtomicProposalManagementServiceImpl(SubjectStateManager<TSubject> subjectStateManager) {
         this.subjectStateManager = subjectStateManager;
      }

      public void EnqueueProposal(TSubject key, Proposal<TSubject> proposal) {
         var subjectState = subjectStateManager.GetOrCreate(key);
         subjectState.EnqueueProposal(proposal);
      }
   }
}