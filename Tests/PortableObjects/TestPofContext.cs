using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public class TestPofContext : PofContext {
      public TestPofContext() {
         MergeContext(new HydarPofContext());

         RegisterPortableObjectType(10000, typeof(AccountEntry));
         RegisterPortableObjectType(10001, typeof(ArrayBox<>));
      }
   }
}
