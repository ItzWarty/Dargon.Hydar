namespace Dargon.Hydar.Clustering {
   public interface HydarConfiguration {
      int TickIntervalMillis { get; }
   }

   public interface ClusteringConfiguration {
      int MaximumMissedHeartBeatIntervalToElection { get; }
      int ElectionTicksToPromotion { get; }
      long EpochDurationMilliseconds { get; }
   }
}
