using System;
using Dargon.Hydar.Clustering.Messages.Helpers;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Clustering {
   public interface EpochManager {
      EpochDescriptor GetCurrentEpoch();
      EpochDescriptor GetEpochByIdOrNull(Guid epochId);

      void EnterEpoch(DateTimeInterval epochTimeInterval, EpochSummary epochSummary, EpochSummary previousEpochSummary);
   }
}
