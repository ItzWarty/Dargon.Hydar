﻿using System;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Messages.Helpers;

namespace Dargon.Hydar.Proposals.Phases {
   public class FollowerRejectedPhase<TSubject> : ProposalPhaseBase<TSubject> {
      private readonly ProposalState<TSubject> proposalState;
      private readonly SubjectState<TSubject> subjectState;
      private readonly ProposalPhaseFactory<TSubject> proposalPhaseFactory;
      private readonly ProposalMessageSender<TSubject> proposalMessageSender;
      private readonly RejectionReason rejectionReason;

      public FollowerRejectedPhase(ProposalState<TSubject> proposalState, SubjectState<TSubject> subjectState, ProposalPhaseFactory<TSubject> proposalPhaseFactory, ProposalMessageSender<TSubject> proposalMessageSender, RejectionReason rejectionReason) {
         this.proposalState = proposalState;
         this.subjectState = subjectState;
         this.proposalPhaseFactory = proposalPhaseFactory;
         this.proposalMessageSender = proposalMessageSender;
         this.rejectionReason = rejectionReason;
      }

      public override void Initialize() {
         base.Initialize();

         BroadcastFollowerReject();
      }

      public override void HandlePrepare() {
         BroadcastFollowerReject();
      }

      public override void HandleCommit() {
         throw new InvalidOperationException();
      }

      public override void HandleCancel() {
         TransitionToCancelledPhase();
      }

      public override void HandleAccept(Guid senderId) { }

      public override void HandleReject(Guid senderId, RejectionReason rejectionReason) { }

      public override void HandleCommitAcknowledgement(Guid senderId) {
         throw new InvalidOperationException();
      }

      public override void HandleCancelAcknowledgement(Guid senderId) {
         TransitionToCancelledPhase();
      }

      public override bool TryCancel() { return false; }

      private void TransitionToCancelledPhase() {
         var cancelledPhase = proposalPhaseFactory.FollowerCancelledPhase();
         proposalState.Transition(cancelledPhase);
      }

      private void BroadcastFollowerReject() {
         proposalMessageSender.FollowerReject(proposalState.ProposalId, rejectionReason);
      }
   }
}
