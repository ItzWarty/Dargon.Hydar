using ItzWarty.Collections;
using System;

namespace Dargon.Hydar.Peering {
   public interface ReadablePeeringState {
      IReadOnlySet<Guid> EnumeratePeerGuids();
      PeerStatus GetPeerStatus(Guid peerId);
   }
}
