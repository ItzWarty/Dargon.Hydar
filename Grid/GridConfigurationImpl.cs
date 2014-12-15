namespace Dargon.Hydar.Grid {
   public class GridConfigurationImpl : GridConfiguration {
      private readonly int tickIntervalMillis;
      private readonly int ticksToElection;
      private readonly int electionTicksToPromotion;

      public GridConfigurationImpl(int tickIntervalMillis, int ticksToElection, int electionTicksToPromotion) {
         this.tickIntervalMillis = tickIntervalMillis;
         this.ticksToElection = ticksToElection;
         this.electionTicksToPromotion = electionTicksToPromotion;
      }

      public int TickIntervalMillis { get { return tickIntervalMillis; } }
      public int TicksToElection { get { return ticksToElection; } }
      public int ElectionTicksToPromotion { get { return electionTicksToPromotion; } }
   }
}