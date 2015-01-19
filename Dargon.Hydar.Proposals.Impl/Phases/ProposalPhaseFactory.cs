using ItzWarty;

namespace Dargon.Hydar.Proposals.Phases {
   public interface ProposalPhaseFactory<TSubject> {
      IProposalPhase<TSubject> Initial(SubjectState<TSubject> subjectState);
      IProposalPhase<TSubject> AcceptedPhase(SubjectState<TSubject> subjectState);
      IProposalPhase<TSubject> RejectedPhase(SubjectState<TSubject> subjectState);
   }

   public class ProposalPhaseFactoryImpl<TSubject> : ProposalPhaseFactory<TSubject> {
      private readonly ActiveProposalManager<TSubject> activeProposalManager;

      public ProposalPhaseFactoryImpl(ActiveProposalManager<TSubject> activeProposalManager) {
         this.activeProposalManager = activeProposalManager;
      }

      public IProposalPhase<TSubject> Initial(SubjectState<TSubject> subjectState) {
         return new InitialPhase<TSubject>(subjectState, this, activeProposalManager);
      }

      public IProposalPhase<TSubject> AcceptedPhase(SubjectState<TSubject> subjectState) {
         return new FollowerAcceptedPhase<TSubject>(subjectState, this).With(x => x.Initialize());
      }

      public IProposalPhase<TSubject> RejectedPhase(SubjectState<TSubject> subjectState) {
         return new FollowerRejectedPhase<TSubject>(subjectState).With(x => x.Initialize());
      }
   }
}
