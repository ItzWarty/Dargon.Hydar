using System;
using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Messages.Helpers;
using ItzWarty.Collections;

namespace Dargon.Hydar.Proposals.Phases {
   public class LeaderCommittedPhase<TSubject> : ProposalPhaseBase<TSubject> {
      private readonly ProposalState<TSubject> proposalState;
      private readonly SubjectState<TSubject> subjectState;
      private readonly ProposalMessageSender<TSubject> proposalMessageSender;
      private readonly ISet<Guid> committedAcknowledgers = new HashSet<Guid>();

      public LeaderCommittedPhase(ProposalState<TSubject> proposalState, SubjectState<TSubject> subjectState, ProposalMessageSender<TSubject> proposalMessageSender) {
         this.proposalState = proposalState;
         this.subjectState = subjectState;
         this.proposalMessageSender = proposalMessageSender;
      }

      public override void HandleEnter() {
         base.HandleEnter();

         BroadcastLeaderCommit();
         subjectState.ExecuteProposal(proposalState.Proposal);
         subjectState.HandleProposalTermination(proposalState);
      }

      public override void HandlePrepare() {
         throw new InvalidOperationException();
      }

      public override void HandleCommit() {
         throw new InvalidOperationException();
      }

      public override void HandleCancel() {
         throw new InvalidOperationException();
      }

      public override void HandleAccept(Guid senderId) { }

      public override void HandleReject(Guid senderId, RejectionReason rejectionReason) { }

      public override void HandleCommitAcknowledgement(Guid senderId) {
         committedAcknowledgers.Add(senderId);
      }

      public override void HandleCancelAcknowledgement(Guid senderId) { }

      public override bool TryCancel() {
         return true;
      }

      private void BroadcastLeaderCommit() {
         proposalMessageSender.LeaderCancel(proposalState.ProposalId);
      }
   }
}