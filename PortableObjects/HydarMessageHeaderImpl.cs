using System;
using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   class HydarMessageHeaderImpl : HydarMessageHeader, IPortableObject {
      private Guid senderGuid;

      public HydarMessageHeaderImpl() { }

      public HydarMessageHeaderImpl(Guid senderGuid) {
         this.senderGuid = senderGuid;
      }

      public Guid SenderGuid { get { return senderGuid; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, senderGuid);
      }

      public void Deserialize(IPofReader reader) {
         senderGuid = reader.ReadGuid(0);
      }
   }
}