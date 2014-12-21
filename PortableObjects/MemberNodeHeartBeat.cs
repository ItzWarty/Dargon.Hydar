using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public class MemberNodeHeartBeat : IPortableObject {
      private Guid epochId;

      public MemberNodeHeartBeat(Guid epochId) {
         this.epochId = epochId;
      }

      public Guid EpochId { get { return epochId; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, epochId);
      }

      public void Deserialize(IPofReader reader) {
         epochId = reader.ReadGuid(0);
      }
   }
}
