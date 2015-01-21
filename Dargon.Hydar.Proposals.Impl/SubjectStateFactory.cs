namespace Dargon.Hydar.Proposals {
   public interface SubjectStateFactory<TSubject> {
      SubjectState<TSubject> Create(TSubject subject);
   }

   public class SubjectStateFactoryImpl<TSubject> : SubjectStateFactory<TSubject> {
      private readonly AtomicExecutionContext<TSubject> atomicExecutionContext;
      private ProposalStateManager<TSubject> proposalStateManager;

      public SubjectStateFactoryImpl(AtomicExecutionContext<TSubject> atomicExecutionContext) {
         this.atomicExecutionContext = atomicExecutionContext;
      }

      public void SetProposalStateManager(ProposalStateManager<TSubject> proposalStateManager) {
         this.proposalStateManager = proposalStateManager;
      }

      public SubjectState<TSubject> Create(TSubject subject) {
         return new SubjectStateImpl<TSubject>(atomicExecutionContext, subject, proposalStateManager);
      }
   }
}