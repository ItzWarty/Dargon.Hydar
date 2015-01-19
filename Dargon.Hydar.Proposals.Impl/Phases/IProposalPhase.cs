using Dargon.Hydar.Networking.PortableObjects;

namespace Dargon.Hydar.Proposals.Phases {
   public interface IProposalPhase<K> {
      void HandleEnter();
      void Step();

      void HandlePrepare();
      void HandleCommit();
      void HandleCancel();
      void HandleAccept();
      void HandleReject();
      bool TryCancel();
   }
}