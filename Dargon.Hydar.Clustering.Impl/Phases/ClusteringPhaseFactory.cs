using System;
using Dargon.Hydar.Clustering.Phases.Helpers;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering.Phases {
   public interface ClusteringPhaseFactory {
      IPhase CreateIndeterminatePhase();
      IPhase CreateInitializationPhase();
      IPhase CreateElectionCandidatePhase(ElectionState electionState, Guid lastEpochId);
      IPhase CreateElectionFollowerPhase(ElectionState electionState);
      IPhase CreateLeaderPhase(IReadOnlySet<Guid> participants);
      IPhase CreateFollowerPhase();
      IPhase CreateDroppedPhase(DateTime end);
   }
}
