namespace Dargon.Hydar.Proposals {
   public interface SubjectState<TSubject> {
      void EnqueueProposal(Proposal<TSubject> proposal);
      void ExecuteProposal(Proposal<TSubject> proposal);
      void HandleProposalTermination(ProposalState<TSubject> proposalState);
      bool TryBullyCurrentProposal(ProposalState<TSubject> suggestedProposalState);
   }
}
