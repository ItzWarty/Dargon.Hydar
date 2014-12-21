using System;

namespace Dargon.Hydar.Clustering {
   public interface PeerStatus {
      Guid Id { get; }
      bool IsLeader { get; }
      int Rank { get; }
      long LastHeartBeatTime { get; }
      long MaxHeartBeatInterval { get; }
      bool IsActive { get; }
   }
}
