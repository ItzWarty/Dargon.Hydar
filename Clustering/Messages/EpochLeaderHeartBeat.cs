using Dargon.Hydar.Clustering.Messages.Helpers;
using Dargon.Hydar.Utilities;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Clustering.Messages {
   public class EpochLeaderHeartBeat : IPortableObject {
      private DateTimeInterval interval;
      private EpochSummary currentEpochSummary;
      private EpochSummary previousEpochSummary;

      public EpochLeaderHeartBeat() { }

      public EpochLeaderHeartBeat(DateTimeInterval interval, EpochSummary currentEpochSummary, EpochSummary previousEpochSummary) {
         this.interval = interval;
         this.currentEpochSummary = currentEpochSummary;
         this.previousEpochSummary = previousEpochSummary;
      }

      public DateTimeInterval Interval { get { return interval; } }
      public EpochSummary CurrentEpochSummary { get { return currentEpochSummary; } }
      public EpochSummary PreviousEpochSummary { get { return previousEpochSummary; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteObject(0, interval);
         writer.WriteObject(1, currentEpochSummary);
         writer.WriteObject(2, previousEpochSummary);
      }

      public void Deserialize(IPofReader reader) {
         interval = reader.ReadObject<DateTimeInterval>(0);
         currentEpochSummary = reader.ReadObject<EpochSummary>(1);
         previousEpochSummary = reader.ReadObject<EpochSummary>(2);
      }
   }
}
