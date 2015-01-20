using System;
using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Messages.Helpers;

namespace Dargon.Hydar.Proposals.Phases {
   public class LeaderCancelledPhase<TSubject> : ProposalPhaseBase<TSubject> {
      private readonly ProposalState<TSubject> proposalState;
      private readonly SubjectState<TSubject> subjectState;
      private readonly ProposalPhaseFactory<TSubject> proposalPhaseFactory;
      private readonly ProposalMessageSender<TSubject> proposalMessageSender;

      public LeaderCancelledPhase(ProposalState<TSubject> proposalState, SubjectState<TSubject> subjectState, ProposalPhaseFactory<TSubject> proposalPhaseFactory, ProposalMessageSender<TSubject> proposalMessageSender) {
         this.proposalState = proposalState;
         this.subjectState = subjectState;
         this.proposalPhaseFactory = proposalPhaseFactory;
         this.proposalMessageSender = proposalMessageSender;
      }

      public override void Initialize() {
         base.Initialize();

         BroadcastLeaderCancel();
      }

      public override void HandlePrepare() { }

      public override void HandleCommit() {
         throw new InvalidOperationException();
      }

      public override void HandleCancel() {
         throw new InvalidOperationException();
      }

      public override void HandleAccept(Guid senderId) { }

      public override void HandleReject(Guid senderId, RejectionReason rejectionReason) { }

      public override void HandleCommitAcknowledgement(Guid senderId) {
         throw new InvalidOperationException();
      }

      public override void HandleCancelAcknowledgement(Guid senderId) {
         throw new InvalidOperationException();
      }

      public override bool TryCancel() {
         throw new InvalidOperationException();
      }

      private void BroadcastLeaderCancel() {
         proposalMessageSender.LeaderCancel(proposalState.ProposalId);
      }
   }
}