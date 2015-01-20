using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Messages.Helpers;

namespace Dargon.Hydar.Proposals.Phases {
   public class FollowerCommittedPhase<TSubject> : ProposalPhaseBase<TSubject> {
      private readonly ProposalState<TSubject> proposalState;
      private readonly SubjectState<TSubject> subjectState;
      private readonly ProposalMessageSender<TSubject> proposalMessageSender;

      public FollowerCommittedPhase(ProposalState<TSubject> proposalState, SubjectState<TSubject> subjectState, ProposalMessageSender<TSubject> proposalMessageSender) {
         this.proposalState = proposalState;
         this.subjectState = subjectState;
         this.proposalMessageSender = proposalMessageSender;
      }

      public override void HandlePrepare() { }

      public override void HandleCommit() {
         SendCommitAcknowledgement();
      }

      public override void HandleCancel() {
         throw new InvalidOperationException();
      }

      public override void HandleAccept(Guid senderId) { }

      public override void HandleReject(Guid senderId, RejectionReason rejectionReason) { }

      public override void HandleCommitAcknowledgement(Guid senderId) { }

      public override void HandleCancelAcknowledgement(Guid senderId) {
         throw new InvalidOperationException();
      }

      public override bool TryCancel() {
         return true;
      }

      private void SendCommitAcknowledgement() {
         proposalMessageSender.FollowerCommitAcknowledgement(proposalState.ProposalId);
      }
   }
}
