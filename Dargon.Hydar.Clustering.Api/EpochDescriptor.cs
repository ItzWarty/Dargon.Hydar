using System;
using Dargon.Hydar.Utilities;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering {
   public interface EpochDescriptor {
      Guid Id { get; }
      Guid LeaderId { get; }
      DateTimeInterval Interval { get; }
      IReadOnlySet<Guid> ParticipantIds { get; } 
      EpochDescriptor Previous { get; }

      EpochSummary ToEpochSummary();
   }
}