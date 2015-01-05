using System;
using Dargon.Hydar.Peering.Messages;

namespace Dargon.Hydar.Peering {
   public interface ManageablePeeringState : ReadablePeeringState {
      void HandlePeeringAnnounce(Guid senderId, uint senderAddress, PeeringAnnounce peeringAnnounce);
   }
}