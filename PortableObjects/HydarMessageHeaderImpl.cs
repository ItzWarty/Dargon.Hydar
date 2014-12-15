using System;

namespace Dargon.Hydar.PortableObjects {
   class HydarMessageHeaderImpl : HydarMessageHeader {
      private readonly Guid senderGuid;

      public HydarMessageHeaderImpl(Guid senderGuid) {
         this.senderGuid = senderGuid;
      }

      public Guid SenderGuid { get { return senderGuid; } }
   }
}