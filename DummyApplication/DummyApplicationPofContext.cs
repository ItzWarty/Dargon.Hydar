using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.PortableObjects;
using DummyApplication.Hydar;

namespace DummyApplication {
   public class DummyApplicationPofContext : PofContext {
      private int kBasePofId = 50000;

      public DummyApplicationPofContext() {
         RegisterPortableObjectType(kBasePofId + 0, typeof(ToLowerProcessor));
      }
   }
}
