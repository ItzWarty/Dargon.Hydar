using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Clustering.Peering {
   public interface IPeeringPhase {
      void Enter();
      void Tick();
      bool Process(IRemoteIdentity sender, HydarMessage message);
   }
}