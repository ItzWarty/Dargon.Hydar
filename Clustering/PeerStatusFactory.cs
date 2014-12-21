using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Clustering {
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
