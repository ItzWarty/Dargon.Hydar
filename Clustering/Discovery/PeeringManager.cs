using System;

namespace Dargon.Hydar.Clustering.Discovery {
   public interface PeeringManager {
      void HandlePeerHeartBeat(Guid peerIdentifier);
   }
}
