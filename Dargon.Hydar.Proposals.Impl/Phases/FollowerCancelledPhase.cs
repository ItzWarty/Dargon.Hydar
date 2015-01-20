using System;
using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Messages.Helpers;

namespace Dargon.Hydar.Proposals.Phases {
   public class FollowerCancelledPhase<TSubject> : ProposalPhaseBase<TSubject> {
      private readonly ProposalState<TSubject> proposalState;
      private readonly SubjectState<TSubject> subjectState;
      private readonly ProposalMessageSender<TSubject> proposalMessageSender;

      public FollowerCancelledPhase(ProposalState<TSubject> proposalState, SubjectState<TSubject> subjectState, ProposalMessageSender<TSubject> proposalMessageSender) {
         this.proposalState = proposalState;
         this.subjectState = subjectState;
         this.proposalMessageSender = proposalMessageSender;
      }

      public override void HandlePrepare() { }

      public override void HandleCommit() {
         throw new InvalidOperationException();
      }

      public override void HandleCancel() {
         SendCancelAcknowledgement();
      }

      public override void HandleAccept(Guid senderId) { }

      public override void HandleReject(Guid senderId, RejectionReason rejectionReason) { }

      public override void HandleCommitAcknowledgement(Guid senderId) {
         throw new InvalidOperationException();
      }

      public override void HandleCancelAcknowledgement(Guid senderId) { }

      public override bool TryCancel() {
         return true;
      }

      private void SendCancelAcknowledgement() {
         proposalMessageSender.FollowerCancelAcknowledgement(proposalState.ProposalId);
      }
   }
}