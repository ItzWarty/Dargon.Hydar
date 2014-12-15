using System;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Grid {
   public interface HydarNode {
      Guid Identifier { get; }
      void Receive(IRemoteIdentity senderIdentity, HydarMessage message);
   }
}