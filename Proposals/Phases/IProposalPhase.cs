using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Proposals.Phases {
   public interface IProposalPhase<K, V> {
      void HandleEnter();
      void Step();
      bool Process(InboundEnvelope envelope);
      bool TryBullyWith(ProposalContext<K, V> candidate);
   }
}