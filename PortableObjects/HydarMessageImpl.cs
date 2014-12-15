using System;
using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public class HydarMessageImpl<TPayload> : HydarMessage<TPayload> {
      private HydarMessageHeader header;
      private TPayload payload;

      public HydarMessageImpl() { }

      public HydarMessageImpl(HydarMessageHeader header, TPayload payload) {
         this.header = header;
         this.payload = payload;
      }

      public void Serialize(IPofWriter writer) {
         writer.WriteObject(0, header);
         writer.WriteObject(1, payload);
      }

      public void Deserialize(IPofReader reader) {
         header = (HydarMessageHeader)reader.ReadObject(0);
         payload = reader.ReadObject<TPayload>(1);
      }

      public HydarMessageHeader Header { get { return header; } }
      public TPayload Payload { get { return payload; } }
      object HydarMessage.Payload { get { return payload; } }
   }
}