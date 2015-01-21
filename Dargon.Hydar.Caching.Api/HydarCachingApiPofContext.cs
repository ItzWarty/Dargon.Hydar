using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Caching.Processors;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Caching {
   public class HydarCachingApiPofContext : PofContext {
      private const int kBasePofId = 2300;

      public HydarCachingApiPofContext() {
         RegisterPortableObjectType(kBasePofId + 0, typeof(PutEntryProcessor<,>));
         RegisterPortableObjectType(kBasePofId + 1, typeof(RemoveEntryProcessor<,>));
      }
   }
}
