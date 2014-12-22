using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Caching {
   public interface CacheConfiguration {
      int Redundancy { get; }
   }

   public class CacheConfigurationImpl : CacheConfiguration {
      private int redundancy = 3;
      public int Redundancy { get { return redundancy; } set { redundancy = value; } }
   }
}
