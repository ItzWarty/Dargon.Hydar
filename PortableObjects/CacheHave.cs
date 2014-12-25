using Dargon.PortableObjects;
using System;
using System.Collections.Generic;

namespace Dargon.Hydar.PortableObjects {
   public class CacheHave<TKey, TValue> : IPortableObject {
      private Guid epochId;
      private Guid requestId;
      private IReadOnlyDictionary<TKey, TValue> results;

      public CacheHave(Guid epochId, Guid requestId, IReadOnlyDictionary<TKey, TValue> results) {
         this.epochId = epochId;
         this.requestId = requestId;
         this.results = results;
      }

      public Guid EpochId { get { return epochId; } }
      public Guid RequestId { get { return requestId; } }
      public IReadOnlyDictionary<TKey, TValue> Results { get { return results; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, epochId);
         writer.WriteGuid(1, requestId);
         writer.WriteMap(2, results);
      }

      public void Deserialize(IPofReader reader) {
         epochId = reader.ReadGuid(0);
         requestId = reader.ReadGuid(1);
         results = (IReadOnlyDictionary<TKey, TValue>)reader.ReadMap<TKey, TValue>(2);
      }
   }
}
