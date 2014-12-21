using System;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering.Peering {
   public interface NodePhaseFactory {
      IPeeringPhase CreateIndeterminatePhase();
      IPeeringPhase CreateInitializationPhase();
      IPeeringPhase CreateElectionPhase();
      IPeeringPhase CreateLeaderPhase(ISet<Guid> participants);
      IPeeringPhase CreateMemberPhase();
   }
}
