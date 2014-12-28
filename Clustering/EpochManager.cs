using System;
using Dargon.Hydar.Clustering.Messages.Helpers;
using Dargon.Hydar.Utilities;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering {
   public interface EpochManager {
      event EpochBeginHandler EpochBegin;

      EpochDescriptor GetCurrentEpoch();
      EpochDescriptor GetEpochByIdOrNull(Guid epochId);

      void EnterEpoch(DateTimeInterval epochTimeInterval, EpochSummary epochSummary, EpochSummary previousEpochSummary);
   }

   public delegate void EpochBeginHandler(EpochManager epochManager, EpochDescriptor epochDescriptor);

   public class EpochManagerImpl : EpochManager {
      public event EpochBeginHandler EpochBegin;
      private readonly ConcurrentDictionary<Guid, EpochDescriptor> epochsById = new ConcurrentDictionary<Guid, EpochDescriptor>(); 
      private EpochDescriptor currentEpoch = null;

      public void Initialize() {
         epochsById.TryAdd(Guid.Empty, EpochDescriptorImpl.kNullEpochDescriptor);
      }

      public EpochDescriptor GetCurrentEpoch() {
         return currentEpoch;
      }

      public EpochDescriptor GetEpochByIdOrNull(Guid epochId) {
         return epochsById.GetValueOrDefault(epochId);
      }

      public void EnterEpoch(DateTimeInterval epochTimeInterval, EpochSummary epochSummary, EpochSummary previousEpochSummary) {
         if (!epochsById.ContainsKey(epochSummary.EpochId)) {
            currentEpoch = GetOrCreateEpoch(epochSummary, previousEpochSummary);
            var capture = EpochBegin;
            if (capture != null) {
               capture(this, currentEpoch);
            }
         }
      }

      private EpochDescriptor GetOrCreateEpoch(EpochSummary epochSummary, EpochSummary previousEpochSummary) {
         return epochsById.GetOrAdd(
            epochSummary.EpochId,
            epochId => {
               return new EpochDescriptorImpl(epochSummary.EpochId, epochSummary.LeaderId, epochSummary.Interval, epochSummary.ParticipantIds, GetOrCreateEpoch(previousEpochSummary, EpochSummary.kNullEpochSummary));
            });
      }
   }
}
