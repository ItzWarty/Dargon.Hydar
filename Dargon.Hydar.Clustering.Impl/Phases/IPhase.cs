using Dargon.Hydar.Networking.PortableObjects;

namespace Dargon.Hydar.Clustering.Phases {
   public interface IPhase {
      void Enter();
      void Tick();
      bool Process(InboundEnvelope envelope);
   }
}