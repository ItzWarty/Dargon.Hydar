namespace Dargon.Hydar.Proposals {
   public interface SubjectState<TSubject> {
      /// <summary>
      /// Signals the Subject State to wake up and consider proposals.
      /// </summary>
      void Signal();

      void EnqueueProposal(Proposal<TSubject> proposal);
      void ExecuteProposal(Proposal<TSubject> proposal);
      void HandleProposalTermination(ProposalState<TSubject> proposalState);
      bool TryBullyCurrentProposal(ProposalState<TSubject> suggestedProposalState);
   }
}
