namespace Dargon.Hydar.Clustering {
   public interface ClusteringConfiguration {
      int MaximumMissedHeartBeatIntervalToElection { get; }
      int ElectionTicksToPromotion { get; }
      long EpochDurationMilliseconds { get; }
   }
}
