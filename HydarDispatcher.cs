using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar {
   public interface HydarDispatcher {
      bool Dispatch(IRemoteIdentity senderIdentity, HydarMessage message);
   }
}
