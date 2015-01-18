using Dargon.Audits;

namespace Dargon.Hydar.Clustering.Phases {
   public class InitializationPhase : PhaseBase {
      private readonly ClusteringPhaseManager clusteringPhaseManager;
      private readonly ClusteringPhaseFactory clusteringPhaseFactory;

      public InitializationPhase(ClusteringPhaseManager clusteringPhaseManager, ClusteringPhaseFactory clusteringPhaseFactory) {
         this.clusteringPhaseManager = clusteringPhaseManager;
         this.clusteringPhaseFactory = clusteringPhaseFactory;
      }

      public override void Enter() {
         var indeterminatePhase = clusteringPhaseFactory.CreateIndeterminatePhase();
         clusteringPhaseManager.Transition(indeterminatePhase);
      }

      public override void Tick() {
         // do nothing
      }
   }
}
