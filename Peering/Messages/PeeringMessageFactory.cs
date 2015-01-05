using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Peering.Messages {
   public interface PeeringMessageFactory {
      PeeringAnnounce PeeringAnnounce();
   }

   public class PeeringMessageFactoryImpl : PeeringMessageFactory {
      public PeeringAnnounce PeeringAnnounce() {
         return new PeeringAnnounce();
      }
   }
}
