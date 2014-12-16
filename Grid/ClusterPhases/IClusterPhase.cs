using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Grid.ClusterPhases {
   public interface IClusterPhase {
      void Enter();
      void Tick();
      bool Process(IRemoteIdentity sender, HydarMessage message);
   }
}