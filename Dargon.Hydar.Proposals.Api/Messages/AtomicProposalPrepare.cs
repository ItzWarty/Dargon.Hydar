namespace Dargon.Hydar.Proposals.Messages {
   public interface AtomicProposalPrepare<TSubject> : AtomicProposalMessage {
      Proposal<TSubject> Proposal { get; }
   }
}
