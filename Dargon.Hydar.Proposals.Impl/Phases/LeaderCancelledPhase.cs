using System;
using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Messages.Helpers;
using ItzWarty.Collections;

namespace Dargon.Hydar.Proposals.Phases {
   public class LeaderCancelledPhase<TSubject> : ProposalPhaseBase<TSubject> {
      private readonly ProposalState<TSubject> proposalState;
      private readonly SubjectState<TSubject> subjectState;
      private readonly ProposalMessageSender<TSubject> proposalMessageSender;
      private readonly ISet<Guid> cancellationAcknowledgers = new HashSet<Guid>();

      public LeaderCancelledPhase(ProposalState<TSubject> proposalState, SubjectState<TSubject> subjectState, ProposalMessageSender<TSubject> proposalMessageSender) {
         this.proposalState = proposalState;
         this.subjectState = subjectState;
         this.proposalMessageSender = proposalMessageSender;
      }

      public override void HandleEnter() {
         base.HandleEnter();

         BroadcastLeaderCancel();
         subjectState.HandleProposalTermination(proposalState);
         subjectState.EnqueueProposal(proposalState.Proposal);
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
         cancellationAcknowledgers.Add(senderId);
      }

      public override bool TryCancel() {
         return true;
      }

      private void BroadcastLeaderCancel() {
         proposalMessageSender.LeaderCancel(proposalState.ProposalId);
      }
   }
}