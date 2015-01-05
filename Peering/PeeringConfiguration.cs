using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Peering {
   public interface PeeringConfiguration {
      long MaximumMissedHeartBeatIntervalToInactivity { get; }
   }

   public class PeeringConfigurationImpl : PeeringConfiguration {
      private readonly long maximumMissedHeartBeatIntervalToInactivity;

      public PeeringConfigurationImpl(long maximumMissedHeartBeatIntervalToInactivity) {
         this.maximumMissedHeartBeatIntervalToInactivity = maximumMissedHeartBeatIntervalToInactivity;
      }

      public long MaximumMissedHeartBeatIntervalToInactivity { get { return maximumMissedHeartBeatIntervalToInactivity; } }
   }
}
