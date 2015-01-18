using Dargon.Hydar.Caching.Data.Operations;
using Dargon.Hydar.Peering.Messages;
using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public class HydarPofContext : PofContext {
      private const int kBasePofId = 2000;

      public HydarPofContext() {
         // [200, 300) peering stuff
         RegisterPortableObjectType(kBasePofId + 300, typeof(PeeringAnnounce));

         // [300, 400) caching stuff
         RegisterPortableObjectType(kBasePofId + 400, typeof(EntryReadOperation<,>));
         RegisterPortableObjectType(kBasePofId + 401, typeof(EntryWriteOperation<,>));
         RegisterPortableObjectType(kBasePofId + 402, typeof(EntryProcessOperation<,,>));
      }
   }
}
