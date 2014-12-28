using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;
using ItzWarty.Collections;
using System;
using Dargon.Hydar.Clustering.Messages;
using Dargon.Hydar.Clustering.Messages.Helpers;
using Dargon.Hydar.Clustering.Phases.Helpers;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Clustering.Phases {
   public class LeaderPhase : PhaseBase {
      private readonly HydarIdentity identity;
      private readonly DebugEventRouter debugEventRouter;
      private readonly EpochManager epochManager;
      private readonly ClusteringConfiguration clusteringConfiguration;
      private readonly ClusteringPhaseManager clusteringPhaseManager;
      private readonly ClusteringPhaseFactory clusteringPhaseFactory;
      private readonly ClusteringMessageSender clusteringMessageSender;
      private readonly IReadOnlySet<Guid> participants;

      public LeaderPhase(HydarIdentity identity, DebugEventRouter debugEventRouter, EpochManager epochManager, ClusteringConfiguration clusteringConfiguration, ClusteringPhaseManager clusteringPhaseManager, ClusteringPhaseFactory clusteringPhaseFactory, ClusteringMessageSender clusteringMessageSender, IReadOnlySet<Guid> participants) {
         this.identity = identity;
         this.debugEventRouter = debugEventRouter;
         this.epochManager = epochManager;
         this.clusteringConfiguration = clusteringConfiguration;
         this.clusteringPhaseManager = clusteringPhaseManager;
         this.clusteringPhaseFactory = clusteringPhaseFactory;
         this.clusteringMessageSender = clusteringMessageSender;
         this.participants = participants;
      }

      public override void Initialize() {
         base.Initialize();
         RegisterNullHandler<ElectionAcknowledgement>();
         RegisterNullHandler<ElectionVote>();
      }

      public override void Enter() {
         base.Enter();

         var lastEpoch = epochManager.GetCurrentEpoch();
         var epochId = Guid.NewGuid();
         var epochStartTime = DateTime.Now;
         var epochExpirationTime = epochStartTime + TimeSpan.FromMilliseconds(clusteringConfiguration.EpochDurationMilliseconds);
         var epochTimeInterval = new DateTimeInterval(epochStartTime, epochExpirationTime);
         var epochSummary = new EpochSummary(epochId, identity.NodeId, participants, epochTimeInterval);
         epochManager.EnterEpoch(epochTimeInterval, epochSummary, lastEpoch == null ? EpochSummary.kNullEpochSummary : lastEpoch.ToEpochSummary());

         SendHeartBeat();
      }

      public override void Tick() {
         var epoch = epochManager.GetCurrentEpoch();
         if (DateTime.Now >= epoch.Interval.End) {
            var electionPhase = clusteringPhaseFactory.CreateElectionCandidatePhase(new ElectionStateImpl(), epoch.Id);
            clusteringPhaseManager.Transition(electionPhase);
         }
         SendHeartBeat();
      }

      private void SendHeartBeat() {
         debugEventRouter.LeaderPhase_SendHeartBeat(participants.Count);
         var epoch = epochManager.GetCurrentEpoch();
         var previousEpoch = epoch.Previous;
         clusteringMessageSender.EpochLeaderHeartBeat(epoch.Interval, epoch.ToEpochSummary(), previousEpoch.ToEpochSummary());
      }
   }
}