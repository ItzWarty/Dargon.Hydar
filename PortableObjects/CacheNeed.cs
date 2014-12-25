using Dargon.PortableObjects;
using ItzWarty.Collections;
using System;

namespace Dargon.Hydar.PortableObjects {
   public class CacheNeed<TKey> : IPortableObject {
      private Guid epochId;
      private Guid requestId;
      private IReadOnlySet<TKey> keys;

      public CacheNeed(Guid epochId, Guid requestId, IReadOnlySet<TKey> keys) {
         this.epochId = epochId;
         this.requestId = requestId;
         this.keys = keys;
      }

      public Guid EpochId { get { return epochId; } }
      public Guid RequestId { get { return requestId; } }
      public IReadOnlySet<TKey> Keys { get { return keys; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, epochId);
         writer.WriteGuid(1, requestId);
         writer.WriteCollection(2, keys);
      }

      public void Deserialize(IPofReader reader) {
         epochId = reader.ReadGuid(0);
         requestId = reader.ReadGuid(1);
         keys = reader.ReadCollection<TKey, HashSet<TKey>>(2);
      }
   }
}
