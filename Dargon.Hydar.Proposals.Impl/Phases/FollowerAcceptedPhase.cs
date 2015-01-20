using System;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Messages.Helpers;

namespace Dargon.Hydar.Proposals.Phases {
   public class FollowerAcceptedPhase<TSubject> : ProposalPhaseBase<TSubject> {
      private readonly ProposalState<TSubject> proposalState;
      private readonly SubjectState<TSubject> subjectState;
      private readonly ProposalPhaseFactory<TSubject> proposalPhaseFactory;
      private readonly ProposalMessageSender<TSubject> proposalMessageSender;

      public FollowerAcceptedPhase(ProposalState<TSubject> proposalState, SubjectState<TSubject> subjectState, ProposalPhaseFactory<TSubject> proposalPhaseFactory, ProposalMessageSender<TSubject> proposalMessageSender) {
         this.proposalState = proposalState;
         this.subjectState = subjectState;
         this.proposalPhaseFactory = proposalPhaseFactory;
         this.proposalMessageSender = proposalMessageSender;
      }

      public override void HandleEnter() {
         base.HandleEnter();

         BroadcastFollowerAccept();
      }

      public override void Step() {
         base.Step();
      }

      public override void HandlePrepare() {
         BroadcastFollowerAccept();
      }

      public override void HandleCommit() {
         TransitionToCommittedPhase();
      }

      public override void HandleCancel() {
         TransitionToCancelledPhase();
      }

      public override void HandleAccept(Guid senderId) { }

      public override void HandleReject(Guid senderId, RejectionReason rejectionReason) {
         TransitionToCancelledPhase();
      }

      public override void HandleCommitAcknowledgement(Guid senderId) {
         TransitionToCommittedPhase();
      }

      public override void HandleCancelAcknowledgement(Guid senderId) {
         TransitionToCancelledPhase();
      }

      public override bool TryCancel() {
         return false;
      }

      private void TransitionToCommittedPhase() {
         var committedPhase = proposalPhaseFactory.FollowerCommittedPhase();
         proposalState.Transition(committedPhase);
      }

      private void TransitionToCancelledPhase() {
         var committedPhase = proposalPhaseFactory.FollowerCancelledPhase();
         proposalState.Transition(committedPhase);
      }

      private void BroadcastFollowerAccept() {
         proposalMessageSender.FollowerAccept(proposalState.ProposalId);
      }
   }
}
