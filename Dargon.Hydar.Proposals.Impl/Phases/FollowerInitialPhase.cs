using System;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Messages.Helpers;

namespace Dargon.Hydar.Proposals.Phases {
   public class FollowerInitialPhase<TSubject> : ProposalPhaseBase<TSubject> {
      private readonly ProposalState<TSubject> proposalState;
      private readonly SubjectState<TSubject> subjectState;
      private readonly ProposalPhaseFactory<TSubject> proposalPhaseFactory;

      public FollowerInitialPhase(ProposalState<TSubject> proposalState, SubjectState<TSubject> subjectState, ProposalPhaseFactory<TSubject> proposalPhaseFactory) {
         this.proposalState = proposalState;
         this.subjectState = subjectState;
         this.proposalPhaseFactory = proposalPhaseFactory;
      }

      public override void Initialize() {
         base.Initialize();
      }

      public override void HandlePrepare() {
         if (subjectState.TryBullyCurrentProposal(proposalState)) {
            var acceptedPhase = proposalPhaseFactory.FollowerAcceptedPhase();
            proposalState.Transition(acceptedPhase);
         } else {
            var rejectedPhase = proposalPhaseFactory.FollowerRejectedPhase(RejectionReason.Bullied);
            proposalState.Transition(rejectedPhase);
         }
      }

      public override void HandleCommit() { }

      public override void HandleCancel() { }

      public override void HandleAccept(Guid senderId) { }

      public override void HandleReject(Guid senderId, RejectionReason rejectionReason) { }

      public override void HandleCommitAcknowledgement(Guid senderId) { }

      public override void HandleCancelAcknowledgement(Guid senderId) { }

      public override bool TryCancel() {
         return true;
      }
   }
}
