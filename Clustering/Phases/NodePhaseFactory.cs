using System;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering.Phases {
   public interface NodePhaseFactory {
      IPhase CreateIndeterminatePhase();
      IPhase CreateInitializationPhase();
      IPhase CreateElectionPhase();
      IPhase CreateElectionPhase(Guid lastEpochId);
      IPhase CreateLeaderPhase(ISet<Guid> participants);
      IPhase CreateMemberPhase();
      IPhase CreateDroppedPhase(DateTime end);
   }
}
