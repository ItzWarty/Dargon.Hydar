using System;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Proposals.Messages.Helpers;

namespace Dargon.Hydar.Proposals.Phases {
   public interface IProposalPhase<TSubject> {
      void HandleEnter();
      void Step();

      void HandlePrepare();
      void HandleCommit();
      void HandleCancel();
      void HandleAccept(Guid senderId);
      void HandleReject(Guid senderId, RejectionReason rejectionReason);
      void HandleCommitAcknowledgement(Guid senderId);
      void HandleCancelAcknowledgement(Guid senderId);
      bool TryCancel();
   }
}