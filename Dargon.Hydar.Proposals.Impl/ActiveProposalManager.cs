namespace Dargon.Hydar.Proposals {
   public interface ActiveProposalManager<TSubject> {
      bool TryBully(TSubject key, SubjectState<TSubject> candidate);
      bool TryDeactivate(SubjectState<TSubject> subjectState);
   }
}
