using System;
using Dargon.Hydar.Peering.Messages;

namespace Dargon.Hydar.Peering {
   public interface PeerStatus {
      Guid Id { get; }
      long LastHeartBeatTime { get; }
      bool IsActive { get; }
   }

   public interface ManageablePeerStatus : PeerStatus {
      void Update(uint newAddress, PeeringAnnounce peeringAnnounce);
   }
}
