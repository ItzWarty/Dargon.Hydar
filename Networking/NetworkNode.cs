using System;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Networking {
   public interface NetworkNode {
      Guid Identifier { get; }
      void Receive(IRemoteIdentity senderIdentity, HydarMessage message);
   }
}