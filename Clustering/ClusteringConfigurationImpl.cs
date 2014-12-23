namespace Dargon.Hydar.Clustering {
   public class ClusteringConfigurationImpl : ClusteringConfiguration {
      private readonly int tickIntervalMillis;
      private readonly int maximumHeartBeatInterval;
      private readonly int electionTicksToPromotion;
      private readonly long epochDurationMilliseconds;

      public ClusteringConfigurationImpl(int tickIntervalMillis, int maximumHeartBeatInterval, int electionTicksToPromotion, long epochDurationMilliseconds) {
         this.tickIntervalMillis = tickIntervalMillis;
         this.maximumHeartBeatInterval = maximumHeartBeatInterval;
         this.electionTicksToPromotion = electionTicksToPromotion;
         this.epochDurationMilliseconds = epochDurationMilliseconds;
      }

      public int TickIntervalMillis { get { return tickIntervalMillis; } }
      public int MaximumHeartBeatInterval { get { return maximumHeartBeatInterval; } }
      public int ElectionTicksToPromotion { get { return electionTicksToPromotion; } }
      public long EpochDurationMilliseconds { get { return epochDurationMilliseconds; } }
   }
}