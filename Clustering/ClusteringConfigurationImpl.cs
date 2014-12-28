namespace Dargon.Hydar.Clustering {
   public class ClusteringConfigurationImpl : ClusteringConfiguration {
      private readonly int maximumHeartBeatInterval;
      private readonly int electionTicksToPromotion;
      private readonly long epochDurationMilliseconds;

      public ClusteringConfigurationImpl(int maximumHeartBeatInterval, int electionTicksToPromotion, long epochDurationMilliseconds) {
         this.maximumHeartBeatInterval = maximumHeartBeatInterval;
         this.electionTicksToPromotion = electionTicksToPromotion;
         this.epochDurationMilliseconds = epochDurationMilliseconds;
      }

      public int MaximumHeartBeatInterval { get { return maximumHeartBeatInterval; } }
      public int ElectionTicksToPromotion { get { return electionTicksToPromotion; } }
      public long EpochDurationMilliseconds { get { return epochDurationMilliseconds; } }
   }

   public class HydarConfigurationImpl : HydarConfiguration {
      private readonly int tickIntervalMillis;

      public HydarConfigurationImpl(int tickIntervalMillis) {
         this.tickIntervalMillis = tickIntervalMillis;
      }

      public int TickIntervalMillis { get { return tickIntervalMillis; } }
   }
}