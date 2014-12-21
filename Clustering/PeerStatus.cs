using System;

namespace Dargon.Hydar.Clustering {
   public interface PeerStatus {
      Guid Id { get; }
      bool IsLeader { get; }
      int Rank { get; }
      long LastHeartBeatTime { get; }
      bool IsActive { get; }
   }

   public interface ManageablePeerStatus : PeerStatus {
      void HandleNewEpoch(bool isLeader, int rank);
      void HandleHeartBeat();
   }
}
