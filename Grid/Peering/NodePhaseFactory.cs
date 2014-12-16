namespace Dargon.Hydar.Grid.Peering {
   public interface NodePhaseFactory {
      IPeeringPhase CreateIndeterminatePhase();
      IPeeringPhase CreateInitializationPhase();
      IPeeringPhase CreateElectionPhase();
      IPeeringPhase CreateLeaderPhase();
      IPeeringPhase CreateMemberPhase();
   }
}
