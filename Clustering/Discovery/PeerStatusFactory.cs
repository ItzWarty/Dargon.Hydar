using System;

namespace Dargon.Hydar.Clustering.Discovery {
   public interface PeerStatusFactory {
      ManageablePeerStatus Create(Guid guid);
   }

   public class PeerStatusFactoryImpl : PeerStatusFactory {
      private readonly ClusteringConfiguration clusteringConfiguration;

      public PeerStatusFactoryImpl(ClusteringConfiguration clusteringConfiguration) {
         this.clusteringConfiguration = clusteringConfiguration;
      }

      public ManageablePeerStatus Create(Guid guid) {
         return new PeerStatusImpl(clusteringConfiguration, guid);
      }
   }
}
