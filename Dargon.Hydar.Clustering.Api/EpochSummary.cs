using System;
using Dargon.Hydar.Utilities;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering {
   public interface EpochSummary {
      Guid EpochId { get; }
      Guid LeaderId { get; }
      IReadOnlySet<Guid> ParticipantIds { get; }
      DateTimeInterval Interval { get; }
   }
}
