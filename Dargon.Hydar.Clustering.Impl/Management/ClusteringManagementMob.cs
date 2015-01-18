using System;
using ItzWarty;
using System.Linq;
using System.Text;
using Dargon.Hydar.Networking;
using Dargon.Management;

namespace Dargon.Hydar.Clustering.Management {
   public class ClusteringManagementMob {
      private readonly HydarIdentity identity;
      private readonly EpochManager epochManager;

      public ClusteringManagementMob(HydarIdentity identity, EpochManager epochManager) {
         this.identity = identity;
         this.epochManager = epochManager;
      }

      [ManagedOperation]
      public string GetLocalIdentifier() {
         var sb = new StringBuilder();
         sb.AppendLine("ID: " + identity.NodeId);
         sb.AppendLine("Groups: ");
         foreach (var group in identity.GroupsByName) {
            sb.Append("   " + group.Value + " " + group.Key);
         }
         return sb.ToString();
      }

      [ManagedOperation]
      public string GetCurrentEpoch() {
         var epoch = epochManager.GetCurrentEpoch();
         var sb = new StringBuilder();
         sb.AppendLine("ID: " + epoch.Id);
         sb.AppendLine("Interval: " + epoch.Interval.Start + " " + epoch.Interval.End);
         sb.AppendLine("LeaderId: " + epoch.LeaderId);
         sb.AppendLine("Participant Count: " + epoch.ParticipantIds.Count);
         sb.AppendLine("PreviousEpochId: " + (epoch.Previous == null ? Guid.Empty : epoch.Previous.Id));
         return sb.ToString();
      }
   }
}
