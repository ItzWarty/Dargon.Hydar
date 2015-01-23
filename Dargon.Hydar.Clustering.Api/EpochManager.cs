using System;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Clustering {
   public interface EpochManager {
      event EpochBeginHandler EpochBegin;

      EpochDescriptor GetCurrentEpoch();
      EpochDescriptor GetEpochByIdOrNull(Guid epochId);

      void EnterEpoch(DateTimeInterval epochTimeInterval, EpochSummary epochSummary, EpochSummary previousEpochSummary);
   }

   public delegate void EpochBeginHandler(EpochManager epochManager, EpochDescriptor epochDescriptor);
}
