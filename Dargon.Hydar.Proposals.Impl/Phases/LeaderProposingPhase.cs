using System;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Messages.Helpers;
using ItzWarty.Collections;

namespace Dargon.Hydar.Proposals.Phases {
   public class LeaderProposingPhase<TSubject> : ProposalPhaseBase<TSubject> {
      private readonly ProposalState<TSubject> proposalState;
      private readonly SubjectState<TSubject> subjectState;
      private readonly ProposalPhaseFactory<TSubject> proposalPhaseFactory;
      private readonly ProposalMessageSender<TSubject> proposalMessageSender;
      private readonly HashSet<Guid> acceptingPeers = new HashSet<Guid>();

      public LeaderProposingPhase(ProposalState<TSubject> proposalState, SubjectState<TSubject> subjectState, ProposalPhaseFactory<TSubject> proposalPhaseFactory, ProposalMessageSender<TSubject> proposalMessageSender) {
         this.proposalState = proposalState;
         this.subjectState = subjectState;
         this.proposalPhaseFactory = proposalPhaseFactory;
         this.proposalMessageSender = proposalMessageSender;
      }

      public override void Initialize() {
         base.Initialize();
      }

      public override void HandleEnter() {
         base.HandleEnter();

         // We might transition to commit 
         BroadcastPrepareMessage();
         ConsiderCommitTransition();
      }

      public override void HandlePrepare() {
         // This indicates a guid collision by two proposing leaders who generated the same guid.
         throw new InvalidOperationException();
      }

      public override void HandleCommit() {
         // This indicates a collision by two leaders who generated the same guid.
         throw new InvalidOperationException();
      }

      public override void HandleCancel() {
         // This indicates a collision by two leaders who generated the same guid.
         throw new InvalidOperationException();
      }

      public override void HandleAccept(Guid senderId) {
         acceptingPeers.Add(senderId);

         ConsiderCommitTransition();
      }

      public override void HandleReject(Guid senderId, RejectionReason rejectionReason) {
         var cancelPhase = proposalPhaseFactory.LeaderCancelledPhase();
         proposalState.Transition(cancelPhase);
      }

      public override void HandleCommitAcknowledgement(Guid senderId) {
         throw new InvalidOperationException();
      }

      public override void HandleCancelAcknowledgement(Guid senderId) {
         throw new InvalidOperationException();
      }

      public override bool TryCancel() {
         return true;
      }

      private void BroadcastPrepareMessage() {
         var proposal = proposalState.Proposal;
         proposalMessageSender.LeaderPrepare(proposalState.ProposalId, proposal.Subject, proposal);
      }

      private void ConsiderCommitTransition() {
         if (acceptingPeers.Count == proposalState.Proposal.Participants.Count - 1) {
            var commitPhase = proposalPhaseFactory.LeaderCommittedPhase();
            proposalState.Transition(commitPhase);
         }
      }
   }
}