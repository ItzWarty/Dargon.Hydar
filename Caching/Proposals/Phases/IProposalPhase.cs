using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public interface IProposalPhase {
      void HandleEnter();
      void Step();
      bool Process(InboundEnvelope envelope);
   }
}