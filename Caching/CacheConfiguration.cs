using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Caching {
   public interface CacheConfiguration {
      int Redundancy { get; }
   }
}
