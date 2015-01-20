using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Networking.Utilities;
using System;
using Dargon.Hydar.Proposals.Messages.Helpers;

namespace Dargon.Hydar.Proposals.Phases {
   public abstract class ProposalPhaseBase<TSubject> : IProposalPhase<TSubject> {
      public virtual void Initialize() { }
      public virtual void HandleEnter() { }
      public virtual void Step() { }

      public abstract void HandlePrepare();
      public abstract void HandleCommit();
      public abstract void HandleCancel();
      public abstract void HandleAccept(Guid senderId);
      public abstract void HandleReject(Guid senderId, RejectionReason rejectionReason);
      public abstract void HandleCommitAcknowledgement(Guid senderId);
      public abstract void HandleCancelAcknowledgement(Guid senderId);
      public abstract bool TryCancel();
   }
}
