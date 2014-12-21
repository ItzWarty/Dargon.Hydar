namespace Dargon.Hydar.Clustering {
   public class ClusteringConfigurationImpl : ClusteringConfiguration {
      private readonly int tickIntervalMillis;
      private readonly int ticksToElection;
      private readonly int electionTicksToPromotion;

      public ClusteringConfigurationImpl(int tickIntervalMillis, int ticksToElection, int electionTicksToPromotion) {
         this.tickIntervalMillis = tickIntervalMillis;
         this.ticksToElection = ticksToElection;
         this.electionTicksToPromotion = electionTicksToPromotion;
      }

      public int TickIntervalMillis { get { return tickIntervalMillis; } }
      public int TicksToElection { get { return ticksToElection; } }
      public int ElectionTicksToPromotion { get { return electionTicksToPromotion; } }
   }
}