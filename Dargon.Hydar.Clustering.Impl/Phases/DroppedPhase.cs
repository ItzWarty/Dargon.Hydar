using System;
using Dargon.Hydar.Clustering.Messages;
using Dargon.Hydar.Clustering.Phases.Helpers;
using Dargon.Hydar.Networking.PortableObjects;

namespace Dargon.Hydar.Clustering.Phases {
   public class DroppedPhase : PhaseBase {
      private readonly ClusteringPhaseManager clusteringPhaseManager;
      private readonly ClusteringPhaseFactory clusteringPhaseFactory;
      private readonly DateTime epochEndTime;

      public DroppedPhase(ClusteringPhaseManager clusteringPhaseManager, ClusteringPhaseFactory clusteringPhaseFactory, DateTime epochEndTime) {
         this.clusteringPhaseManager = clusteringPhaseManager;
         this.clusteringPhaseFactory = clusteringPhaseFactory;
         this.epochEndTime = epochEndTime;
      }

      public override void Initialize() {
         base.Initialize();

         RegisterNullHandler<EpochLeaderHeartBeat>();
         RegisterNullHandler<ElectionAcknowledgement>();
         RegisterHandler<ElectionVote>(HandleElectionVote);
      }

      public override void Tick() { }

      private void HandleElectionVote(InboundEnvelopeHeader header, ElectionVote message) {
         if (DateTime.Now >= epochEndTime) {
            ElectionState electionState = new ElectionStateImpl();
            Guid lastEpochId = Guid.Empty; // uninitialized to prioritize senior nodes
            var electionPhase = clusteringPhaseFactory.CreateElectionCandidatePhase(electionState, lastEpochId);
            clusteringPhaseManager.Transition(electionPhase);
         }
      }
   }
}
