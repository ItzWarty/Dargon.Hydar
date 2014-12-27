using Dargon.Audits;
using Dargon.Hydar.PortableObjects;
using System;
using System.Linq;
using Dargon.Hydar.Clustering.Messages;
using Dargon.Hydar.Clustering.Messages.Helpers;
using Dargon.Hydar.Clustering.Phases.Helpers;
using ItzWarty.Collections;
using SCG = System.Collections.Generic;

namespace Dargon.Hydar.Clustering.Phases {
   public class ElectionCandidatePhase : PhaseBase {
      private readonly Guid localIdentifier;
      private readonly ElectionState state;
      private readonly Guid lastEpochId;
      private readonly ClusteringConfiguration clusteringConfiguration;
      private readonly ClusteringPhaseFactory clusteringPhaseFactory;
      private readonly ClusteringPhaseManager clusteringPhaseManager;
      private readonly ClusteringMessageSender clusteringMessageSender;
      private readonly object synchronization = new object();
      private readonly ISet<Guid> allParticipants = new HashSet<Guid>();

      private int electionSecurity = 0;
      public ElectionCandidatePhase(Guid localIdentifier, ElectionState state, Guid lastEpochId, ClusteringConfiguration clusteringConfiguration, ClusteringPhaseFactory clusteringPhaseFactory, ClusteringPhaseManager clusteringPhaseManager, ClusteringMessageSender clusteringMessageSender) {
         this.localIdentifier = localIdentifier;
         this.state = state;
         this.lastEpochId = lastEpochId;
         this.clusteringConfiguration = clusteringConfiguration;
         this.clusteringPhaseFactory = clusteringPhaseFactory;
         this.clusteringPhaseManager = clusteringPhaseManager;
         this.clusteringMessageSender = clusteringMessageSender;
      }

      public override void Initialize() {
         base.Initialize();

         state.ConsiderCandidate(new ElectionCandidate(localIdentifier, lastEpochId));

         RegisterHandler<ElectionVote>(HandleElectionVote);
         RegisterHandler<ElectionAcknowledgement>(HandleElectionAcknowledgement);
         RegisterHandler<EpochLeaderHeartBeat>(HandleLeaderHeartBeat);
      }

      public override void Enter() {
         base.Enter();

         lock (synchronization) {
            allParticipants.Add(localIdentifier);
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
         }
      }

      private void HandleElectionVote(InboundEnvelopeHeader header, ElectionVote vote) {
         lock (synchronization) {
            allParticipants.Add(header.SenderId);

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
            if (acknowledgement.AcknowledgedVoter == localIdentifier) {
               state.AddAcknowledger(header.SenderId);
            } else {
               electionSecurity = 0;
            }
         }
      }

      private void HandleLeaderHeartBeat(InboundEnvelopeHeader header, EpochLeaderHeartBeat payload) {
         if (DateTime.Now >= payload.Interval.End) {
            return; // throw away stale message
         }
         if (payload.CurrentEpochSummary.ParticipantIds.Contains(localIdentifier)) {
            var memberPhase = clusteringPhaseFactory.CreateFollowerPhase();
            clusteringPhaseManager.Transition(memberPhase);
         } else {
            var droppedPhase = clusteringPhaseFactory.CreateDroppedPhase(payload.Interval.End);
            clusteringPhaseManager.Transition(droppedPhase);
         }
      }
   }
}
