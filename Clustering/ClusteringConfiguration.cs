namespace Dargon.Hydar.Clustering {
   public interface ClusteringConfiguration {
      int TickIntervalMillis { get; }
      int MaximumHeartBeatInterval { get; }
      int ElectionTicksToPromotion { get; }
      long EpochDurationMilliseconds { get; }
   }
}
