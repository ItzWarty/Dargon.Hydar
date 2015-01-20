using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Messages.Helpers;
using ItzWarty;

namespace Dargon.Hydar.Proposals.Phases {
   public interface ProposalPhaseFactory<TSubject> {
      IProposalPhase<TSubject> FollowerInitialPhase();
      IProposalPhase<TSubject> FollowerAcceptedPhase();
      IProposalPhase<TSubject> FollowerRejectedPhase(RejectionReason rejectionReason);
      IProposalPhase<TSubject> FollowerCommittedPhase();
      IProposalPhase<TSubject> FollowerCancelledPhase();

      IProposalPhase<TSubject> LeaderProposingPhase();
      IProposalPhase<TSubject> LeaderCommittedPhase();
      IProposalPhase<TSubject> LeaderCancelledPhase();
   }

   public class ProposalPhaseFactoryImpl<TSubject> : ProposalPhaseFactory<TSubject> {
      private readonly ProposalState<TSubject> proposalState;
      private readonly SubjectState<TSubject> subjectState;
      private readonly ProposalMessageSender<TSubject> proposalMessageSender;

      public ProposalPhaseFactoryImpl(ProposalState<TSubject> proposalState, SubjectState<TSubject> subjectState, ProposalMessageSender<TSubject> proposalMessageSender) {
         this.proposalState = proposalState;
         this.subjectState = subjectState;
         this.proposalMessageSender = proposalMessageSender;
      }

      public IProposalPhase<TSubject> FollowerInitialPhase() {
         return new FollowerInitialPhase<TSubject>(proposalState, subjectState, this).With(x => x.Initialize());
      }

      public IProposalPhase<TSubject> FollowerAcceptedPhase() {
         return new FollowerAcceptedPhase<TSubject>(proposalState, subjectState, this, proposalMessageSender).With(x => x.Initialize());
      }

      public IProposalPhase<TSubject> FollowerRejectedPhase(RejectionReason rejectionReason) {
         return new FollowerRejectedPhase<TSubject>(proposalState, subjectState, this, proposalMessageSender, rejectionReason).With(x => x.Initialize());
      }

      public IProposalPhase<TSubject> FollowerCommittedPhase() {
         return new FollowerCommittedPhase<TSubject>(proposalState, subjectState, proposalMessageSender).With(x => x.Initialize());
      }

      public IProposalPhase<TSubject> FollowerCancelledPhase() {
         return new FollowerCancelledPhase<TSubject>(proposalState, subjectState, proposalMessageSender).With(x => x.Initialize());
      }

      public IProposalPhase<TSubject> LeaderProposingPhase() {
         return new LeaderProposingPhase<TSubject>(proposalState, subjectState, this, proposalMessageSender);
      }

      public IProposalPhase<TSubject> LeaderCommittedPhase() {
         throw new System.NotImplementedException();
      }

      public IProposalPhase<TSubject> LeaderCancelledPhase() {
         throw new System.NotImplementedException();
      }
   }
}
