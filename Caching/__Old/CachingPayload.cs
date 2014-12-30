using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Caching {
   public interface CachingPayload {
      CachingPayloadMetadata Metadata { get; }
      object InnerPayload { get; }
   }

   public class CachingPayload<TInnerPayload> : IPortableObject, CachingPayload {
      private CachingPayloadMetadata metadata;
      private TInnerPayload innerPayload;

      public CachingPayload() { } 

      public CachingPayload(CachingPayloadMetadata metadata, TInnerPayload innerPayload) {
         this.metadata = metadata;
         this.innerPayload = innerPayload;
      }

      public CachingPayloadMetadata Metadata { get { return metadata; } }
      public TInnerPayload InnerPayload { get { return innerPayload; } }
      object CachingPayload.InnerPayload { get { return innerPayload; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteObject(0, metadata);
         writer.WriteObject(1, innerPayload);
      }

      public void Deserialize(IPofReader reader) {
         metadata = reader.ReadObject<CachingPayloadMetadata>(0);
         innerPayload = reader.ReadObject<TInnerPayload>(1);
      }
   }
}
