using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Grid.Phases {
   public interface IPhase {
      void Enter();
      void Tick();
      void Receive(IRemoteIdentity sender, HydarMessage message);
   }
}