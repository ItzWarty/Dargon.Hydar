using System;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering {
   public interface EpochDescriptor {
      Guid Id { get; }
      Guid LeaderGuid { get; }
      IReadOnlySet<Guid> ParticipantGuids { get; } 
   }
}