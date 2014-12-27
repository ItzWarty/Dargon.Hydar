using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Clustering.Phases {
   public interface IPhase {
      void Enter();
      void Tick();
      bool Process(InboundEnvelope envelope);
   }
}