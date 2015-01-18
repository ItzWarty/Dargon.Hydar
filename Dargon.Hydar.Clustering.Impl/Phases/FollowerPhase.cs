using Dargon.Hydar.Clustering.Messages;
using Dargon.Hydar.Clustering.Phases.Helpers;
using Dargon.Hydar.Utilities;
using System;
using System.Threading;
using Dargon.Hydar.Clustering.Utilities;
using Dargon.Hydar.Networking.PortableObjects;

namespace Dargon.Hydar.Clustering.Phases {
   public class FollowerPhase : PhaseBase {
      private readonly DebugEventRouter debugEventRouter;
      private readonly EpochManager epochManager;
      private readonly ClusteringConfiguration clusteringConfiguration;
      private readonly ClusteringPhaseManager clusteringPhaseManager;
      private readonly ClusteringPhaseFactory clusteringPhaseFactory;
      private int leaderAbsentTicks = 0;

      public FollowerPhase(DebugEventRouter debugEventRouter, EpochManager epochManager, ClusteringConfiguration clusteringConfiguration, ClusteringPhaseManager clusteringPhaseManager, ClusteringPhaseFactory clusteringPhaseFactory) {
         this.debugEventRouter = debugEventRouter;
         this.epochManager = epochManager;
         this.clusteringConfiguration = clusteringConfiguration;
         this.clusteringPhaseManager = clusteringPhaseManager;
         this.clusteringPhaseFactory = clusteringPhaseFactory;
      }

      public override void Initialize() {
         base.Initialize();
         RegisterHandler<EpochLeaderHeartBeat>(HandleLeaderHeartBeat);
         RegisterNullHandler<ElectionAcknowledgement>();
         RegisterNullHandler<ElectionVote>();
      }

      public override void Tick() {
         var currentEpoch = epochManager.GetCurrentEpoch();
         var nextLeaderAbsentTicks = Interlocked.Increment(ref leaderAbsentTicks);
         var shouldTransitionToElection = false;
         if (nextLeaderAbsentTicks > clusteringConfiguration.MaximumMissedHeartBeatIntervalToElection) {
            debugEventRouter.FollowerPhase_LeaderMissedHeartBeats(clusteringConfiguration.MaximumMissedHeartBeatIntervalToElection);
            shouldTransitionToElection = true;
         } else if (DateTime.Now >= currentEpoch.Interval.End) {
            debugEventRouter.FollowerPhase_EndOfEpoch(currentEpoch.Id);
            shouldTransitionToElection = true;
         }
         if (shouldTransitionToElection) {
            var electionPhase = clusteringPhaseFactory.CreateElectionCandidatePhase(new ElectionStateImpl(), currentEpoch.Id);
            clusteringPhaseManager.Transition(electionPhase);
         }
      }

      private void HandleLeaderHeartBeat(InboundEnvelopeHeader header, EpochLeaderHeartBeat heartbeat) {
         Interlocked.Exchange(ref leaderAbsentTicks, 0);
      }
   }
}