using System;
using ItzWarty;

namespace Dargon.Hydar.Clustering {
   public class PeerStatusImpl : ManageablePeerStatus {
      private readonly ClusteringConfiguration clusteringConfiguration;
      private readonly Guid id;
      private bool isLeader = false;
      private int rank = -1;
      private long lastHeartBeatTime = -1;

      public PeerStatusImpl(ClusteringConfiguration clusteringConfiguration, Guid id) {
         this.clusteringConfiguration = clusteringConfiguration;
         this.id = id;
      }

      public void HandleNewEpoch(bool isLeader, int rank) {
         this.isLeader = isLeader;
         this.rank = rank;
      }

      public void HandleHeartBeat() {
         lastHeartBeatTime = Util.GetUnixTimeMilliseconds();
      }

      public Guid Id { get { return id; } }
      public bool IsLeader { get { return isLeader; } }
      public int Rank { get { return rank; } }
      public long LastHeartBeatTime { get { return lastHeartBeatTime; } }
      public bool IsActive { get { return Util.GetUnixTimeMilliseconds() < lastHeartBeatTime + clusteringConfiguration.MaximumHeartBeatInterval; } }
   }
}