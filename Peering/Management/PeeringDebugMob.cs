using Dargon.Management;
using System;
using System.Text;

namespace Dargon.Hydar.Peering.Management {
   public class PeeringDebugMob {
      private readonly ManageablePeeringState manageablePeeringState;

      public PeeringDebugMob(ManageablePeeringState manageablePeeringState) {
         this.manageablePeeringState = manageablePeeringState;
      }

      [ManagedOperation]
      public string GetPeers() {
         var peers = manageablePeeringState.EnumeratePeerGuids();
         var sb = new StringBuilder();
         sb.AppendLine("Peers: ");
         foreach(var peer in peers) {
            sb.AppendLine("   " + peer.ToString("n"));
         }
         return sb.ToString();
      }

      [ManagedOperation]
      public string GetPeerStatus(string guidString) {
         var guid = Guid.Parse(guidString);
         var status = manageablePeeringState.GetPeerStatus(guid);
         var sb = new StringBuilder();
         sb.AppendLine("Status of Node " + guid.ToString("n") + ": ");
         sb.AppendLine("LastHeartBeatTime: " + status.LastHeartBeatTime);
         sb.AppendLine("IsActive: " + status.IsActive);
         return sb.ToString();
      }
   }
}
