namespace Dargon.Hydar.Clustering {
   public class ClusteringConfigurationImpl : ClusteringConfiguration {
      private readonly int maximumMissedHeartBeatIntervalToElection;
      private readonly int electionTicksToPromotion;
      private readonly long epochDurationMilliseconds;

      public ClusteringConfigurationImpl(int maximumMissedHeartBeatIntervalToElection, int electionTicksToPromotion, long epochDurationMilliseconds) {
         this.maximumMissedHeartBeatIntervalToElection = maximumMissedHeartBeatIntervalToElection;
         this.electionTicksToPromotion = electionTicksToPromotion;
         this.epochDurationMilliseconds = epochDurationMilliseconds;
      }

      public int MaximumMissedHeartBeatIntervalToElection { get { return maximumMissedHeartBeatIntervalToElection; } }
      public int ElectionTicksToPromotion { get { return electionTicksToPromotion; } }
      public long EpochDurationMilliseconds { get { return epochDurationMilliseconds; } }
   }
}