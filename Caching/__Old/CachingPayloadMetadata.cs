using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Caching {
   public class CachingPayloadMetadata : IPortableObject {
      private Guid cacheId;
      private Guid epochId;

      public CachingPayloadMetadata() { }

      public CachingPayloadMetadata(Guid cacheId, Guid epochId) {
         this.cacheId = cacheId;
         this.epochId = epochId;
      }

      public Guid CacheId { get { return cacheId; } }
      public Guid EpochId {  get { return epochId; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, cacheId);
         writer.WriteGuid(1, epochId);
      }

      public void Deserialize(IPofReader reader) {
         cacheId = reader.ReadGuid(0);
         epochId = reader.ReadGuid(1);
      }
   }
}
