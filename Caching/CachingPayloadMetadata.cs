using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Caching {
   public class CachingPayloadMetadata : IPortableObject {
      private Guid cacheId;

      public CachingPayloadMetadata() { }

      public CachingPayloadMetadata(Guid cacheId) {
         this.cacheId = cacheId;
      }

      public Guid CacheId { get { return cacheId; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, cacheId);
      }

      public void Deserialize(IPofReader reader) {
         cacheId = reader.ReadGuid(0);
      }
   }
}
