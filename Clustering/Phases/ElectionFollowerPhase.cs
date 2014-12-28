using Dargon.Hydar.Clustering.Messages;
using Dargon.Hydar.Clustering.Phases.Helpers;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.PortableObjects;
using System;
using System.Linq;
using Dargon.Hydar.Clustering.Messages.Helpers;
using Dargon.Hydar.Networking;

namespace Dargon.Hydar.Clustering.Phases {
   public class ElectionFollowerPhase : PhaseBase {
      private readonly Guid localIdentifier;
      private readonly ElectionState state;
      private readonly EpochManager epochManager;
      private readonly ClusteringPhaseFactory clusteringPhaseFactory;
      private readonly ClusteringPhaseManager clusteringPhaseManager;
      private readonly ClusteringMessageSender clusteringMessageSender;
      private bool isVoteStable = false;

      public ElectionFollowerPhase(Guid localIdentifier, ElectionState state, EpochManager epochManager, ClusteringPhaseFactory clusteringPhaseFactory, ClusteringPhaseManager clusteringPhaseManager, ClusteringMessageSender clusteringMessageSender) {
         this.localIdentifier = localIdentifier;
         this.state = state;
         this.epochManager = epochManager;
         this.clusteringPhaseFactory = clusteringPhaseFactory;
         this.clusteringPhaseManager = clusteringPhaseManager;
         this.clusteringMessageSender = clusteringMessageSender;
      }

      public override void Initialize() {
         base.Initialize();

         RegisterHandler<ElectionVote>(HandleElectionVote);
         RegisterHandler<ElectionAcknowledgement>(HandleElectionAcknowledgement);
         RegisterHandler<EpochLeaderHeartBeat>(HandleLeaderHeartBeat);
      }

      public override void Tick() {
         if (isVoteStable && state.IsAcknowledgedBySelectedCandidate()) {
            // do nothing
         } else {
            clusteringMessageSender.ElectionVote(state.SelectedCandidate);
            isVoteStable = true;
         }
      }

      internal void HandleElectionVote(InboundEnvelopeHeader header, ElectionVote vote) {
         var result = state.ConsiderCandidate(vote.Candidate);
         if (result != ConsiderationResult.SuggestionEquivalent) {
            isVoteStable = false;
         }
      }

      internal void HandleElectionAcknowledgement(InboundEnvelopeHeader header, ElectionAcknowledgement ack) {
         if (ack.AcknowledgedVoter == localIdentifier) {
            state.AddAcknowledger(header.SenderId);
         }
      }

      internal void HandleLeaderHeartBeat(InboundEnvelopeHeader header, EpochLeaderHeartBeat heartBeat) {
         if (DateTime.Now < heartBeat.Interval.End) {
            IPhase nextPhase;
            if (heartBeat.CurrentEpochSummary.ParticipantIds.Contains(localIdentifier)) {
               epochManager.EnterEpoch(heartBeat.Interval, heartBeat.CurrentEpochSummary, heartBeat.PreviousEpochSummary);
               nextPhase = clusteringPhaseFactory.CreateFollowerPhase();
            } else {
               nextPhase = clusteringPhaseFactory.CreateDroppedPhase(heartBeat.Interval.End);
            }
            clusteringPhaseManager.Transition(nextPhase);
         }
      }
   }
}
