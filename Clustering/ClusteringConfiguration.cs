namespace Dargon.Hydar.Clustering {
   public interface ClusteringConfiguration {
      int TickIntervalMillis { get; }
      int TicksToElection { get; }
      int ElectionTicksToPromotion { get; }
   }
}
