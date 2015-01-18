using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Utilities {
   public class HydarUtilitiesPofContext : PofContext {
      private const int kBasePofId = 2000;

      public HydarUtilitiesPofContext() {
         RegisterPortableObjectType(kBasePofId + 0, typeof(DateTimeInterval));
      }
   }
}
