using Dargon.Hydar.Peering.Messages;
using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public class HydarPofContext : PofContext {
      private const int kBasePofId = 2000;

      public HydarPofContext() {
         // [200, 300) peering stuff
         RegisterPortableObjectType(kBasePofId + 500, typeof(PeeringAnnounce));
      }
   }
}
