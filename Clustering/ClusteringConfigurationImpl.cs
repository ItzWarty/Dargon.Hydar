namespace Dargon.Hydar.Clustering {
   public class ClusteringConfigurationImpl : ClusteringConfiguration {
      private readonly int tickIntervalMillis;
      private readonly int maximumHeartBeatInterval;
      private readonly int electionTicksToPromotion;

      public ClusteringConfigurationImpl(int tickIntervalMillis, int maximumHeartBeatInterval, int electionTicksToPromotion) {
         this.tickIntervalMillis = tickIntervalMillis;
         this.maximumHeartBeatInterval = maximumHeartBeatInterval;
         this.electionTicksToPromotion = electionTicksToPromotion;
      }

      public int TickIntervalMillis { get { return tickIntervalMillis; } }
      public int MaximumHeartBeatInterval { get { return maximumHeartBeatInterval; } }
      public int ElectionTicksToPromotion { get { return electionTicksToPromotion; } }
   }
}