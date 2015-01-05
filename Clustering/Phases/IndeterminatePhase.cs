using System;
using System.Threading;
using Dargon.Audits;
using Dargon.Hydar.Clustering.Messages;
using Dargon.Hydar.Clustering.Phases.Helpers;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Clustering.Phases {
   public class IndeterminatePhase : PhaseBase {
      private readonly ClusteringConfiguration clusteringConfiguration;
      private readonly ClusteringPhaseManager clusteringPhaseManager;
      private readonly ClusteringPhaseFactory clusteringPhaseFactory;
      private int tickCount = 0;

      public IndeterminatePhase(ClusteringConfiguration clusteringConfiguration, ClusteringPhaseManager clusteringPhaseManager, ClusteringPhaseFactory clusteringPhaseFactory) {
         this.clusteringConfiguration = clusteringConfiguration;
         this.clusteringPhaseManager = clusteringPhaseManager;
         this.clusteringPhaseFactory = clusteringPhaseFactory;
      }

      public override void Initialize() {
         base.Initialize();
         RegisterHandler<EpochLeaderHeartBeat>(HandleLeaderHeartBeat);
         RegisterHandler<ElectionVote>(HandleElectionVote);
         RegisterNullHandler<ElectionAcknowledgement>();
      }

      public override void Tick() {
         var currentTickCount = Interlocked.Increment(ref tickCount);
         if (currentTickCount < clusteringConfiguration.MaximumMissedHeartBeatIntervalToElection) {
            // do nothing
         } else {
            TransitionToElectionPhase();
         }
      }

      internal void HandleLeaderHeartBeat(InboundEnvelopeHeader header, EpochLeaderHeartBeat heartbeat) {
         Interlocked.Exchange(ref tickCount, 0);
      }

      internal void HandleElectionVote(InboundEnvelopeHeader header, ElectionVote vote) {
         TransitionToElectionPhase();
      }

      internal void TransitionToElectionPhase() {
         var electionState = new ElectionStateImpl();
         var electionPhase = clusteringPhaseFactory.CreateElectionCandidatePhase(electionState, Guid.Empty);
         clusteringPhaseManager.Transition(electionPhase);
      }
   }
}
