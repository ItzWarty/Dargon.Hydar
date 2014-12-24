using System;
using Dargon.Hydar.Utilities;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering {
   public interface EpochDescriptor {
      Guid Id { get; }
      DateTimeInterval Interval { get; }
      Guid LeaderGuid { get; }
      IReadOnlySet<Guid> ParticipantGuids { get; } 
      Guid PreviousId { get; }
   }
}