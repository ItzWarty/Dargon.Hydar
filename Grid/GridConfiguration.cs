using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Grid {
   public interface GridConfiguration {
      int TickIntervalMillis { get; }
      int TicksToElection { get; }
      int ElectionTicksToPromotion { get; }
   }
}
