using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Utilities {
   public interface PeriodicTickerConfiguration {
      int TickIntervalMillis { get; set; }
   }

   public class PeriodicTickerConfigurationImpl : PeriodicTickerConfiguration {
      private int tickIntervalMillis;

      public PeriodicTickerConfigurationImpl(int tickIntervalMillis) {
         this.tickIntervalMillis = tickIntervalMillis;
      }

      public int TickIntervalMillis { get { return tickIntervalMillis; } set { tickIntervalMillis = value; } }
   }
}
