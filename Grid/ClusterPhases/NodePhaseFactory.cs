namespace Dargon.Hydar.Grid.ClusterPhases {
   public interface NodePhaseFactory {
      IClusterPhase CreateIndeterminatePhase();
      IClusterPhase CreateInitializationPhase();
      IClusterPhase CreateElectionPhase();
      IClusterPhase CreateLeaderPhase();
      IClusterPhase CreateMemberPhase();
   }
}
