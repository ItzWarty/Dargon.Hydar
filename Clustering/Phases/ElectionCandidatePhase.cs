using Dargon.Audits;
using Dargon.Hydar.PortableObjects;
using System;
using System.Linq;
using Dargon.Hydar.Clustering.Messages;
using Dargon.Hydar.Clustering.Messages.Helpers;
using Dargon.Hydar.Clustering.Phases.Helpers;
using Dargon.Hydar.Utilities;
using ItzWarty.Collections;
using SCG = System.Collections.Generic;

namespace Dargon.Hydar.Clustering.Phases {
   public class ElectionCandidatePhase : PhaseBase {
      private readonly HydarIdentity identity;
      private readonly ElectionState state;
      private readonly Guid lastEpochId;
      private readonly DebugEventRouter debugEventRouter;
      private readonly EpochManager epochManager;
      private readonly ClusteringConfiguration clusteringConfiguration;
      private readonly ClusteringPhaseFactory clusteringPhaseFactory;
      private readonly ClusteringPhaseManager clusteringPhaseManager;
      private readonly ClusteringMessageSender clusteringMessageSender;
      private readonly object synchronization = new object();
      private int electionSecurity = 0;

      public ElectionCandidatePhase(HydarIdentity identity, ElectionState state, Guid lastEpochId, DebugEventRouter debugEventRouter, EpochManager epochManager, ClusteringConfiguration clusteringConfiguration, ClusteringPhaseFactory clusteringPhaseFactory, ClusteringPhaseManager clusteringPhaseManager, ClusteringMessageSender clusteringMessageSender) {
         this.identity = identity;
         this.state = state;
         this.lastEpochId = lastEpochId;
         this.debugEventRouter = debugEventRouter;
         this.epochManager = epochManager;
         this.clusteringConfiguration = clusteringConfiguration;
         this.clusteringPhaseFactory = clusteringPhaseFactory;
         this.clusteringPhaseManager = clusteringPhaseManager;
         this.clusteringMessageSender = clusteringMessageSender;
      }

      public override void Initialize() {
         base.Initialize();

         state.ConsiderCandidate(new ElectionCandidate(identity.NodeId, lastEpochId));

         RegisterHandler<ElectionVote>(HandleElectionVote);
         RegisterHandler<ElectionAcknowledgement>(HandleElectionAcknowledgement);
         RegisterHandler<EpochLeaderHeartBeat>(HandleLeaderHeartBeat);
      }

      public override void Enter() {
         base.Enter();

         lock (synchronization) {
            state.AddParticipant(identity.NodeId);
            clusteringMessageSender.ElectionVote(state.SelectedCandidate);
         }
      }

      public override void Tick() {
         lock (synchronization) {
            clusteringMessageSender.ElectionVote(state.SelectedCandidate);

            if (electionSecurity > clusteringConfiguration.ElectionTicksToPromotion) {
               var leaderPhase = clusteringPhaseFactory.CreateLeaderPhase(state.Participants);
               clusteringPhaseManager.Transition(leaderPhase);
            }

            electionSecurity++;
            Console.WriteLine(electionSecurity);
         }
      }

      private void HandleElectionVote(InboundEnvelopeHeader header, ElectionVote vote) {
         lock (synchronization) {
            state.AddParticipant(header.SenderId);

            var comparisonResult = state.ConsiderCandidate(vote.Candidate);
            if (comparisonResult == ConsiderationResult.SuggestionBetter) {
               var followerPhase = clusteringPhaseFactory.CreateElectionFollowerPhase(state);
               clusteringPhaseManager.Transition(followerPhase);
            } else if (comparisonResult == ConsiderationResult.SuggestionEquivalent) {
               clusteringMessageSender.ElectionAcknowledgement(header.SenderId);
               electionSecurity = 0;
            } else {
               electionSecurity = 0;
            }
         }
      }

      private void HandleElectionAcknowledgement(InboundEnvelopeHeader header, ElectionAcknowledgement acknowledgement) {
         lock (synchronization) {
            if (acknowledgement.AcknowledgedVoter == identity.NodeId) {
               state.AddAcknowledger(header.SenderId);
            } else {
               electionSecurity = 0;
            }
         }
      }

      private void HandleLeaderHeartBeat(InboundEnvelopeHeader header, EpochLeaderHeartBeat heartBeat) {
         if (DateTime.Now >= heartBeat.Interval.End) {
            return; // throw away stale message
         }
         if (heartBeat.CurrentEpochSummary.ParticipantIds.Contains(identity.NodeId)) {
            debugEventRouter.ElectionCandidatePhase_RejoinEpoch(heartBeat.CurrentEpochSummary.EpochId);
            epochManager.EnterEpoch(heartBeat.Interval, heartBeat.CurrentEpochSummary, heartBeat.PreviousEpochSummary);
            var memberPhase = clusteringPhaseFactory.CreateFollowerPhase();
            clusteringPhaseManager.Transition(memberPhase);
         } else {
            var droppedPhase = clusteringPhaseFactory.CreateDroppedPhase(heartBeat.Interval.End);
            clusteringPhaseManager.Transition(droppedPhase);
         }
      }
   }
}
