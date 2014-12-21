using Dargon.Hydar.PortableObjects;
using System;

namespace Dargon.Hydar.Networking {
   public class NetworkNodeImpl : NetworkNode {
      private readonly Guid identifier;
      private HydarContext context;

      public NetworkNodeImpl(Guid identifier) {
         this.identifier = identifier;
      }

      public void SetContext(HydarContext context) {
         this.context = context;
      }

      public Guid Identifier { get { return identifier; } }

      public void Receive(IRemoteIdentity senderIdentity, HydarMessage message) {
         context.Dispatch(senderIdentity, message);
      }
   }
}