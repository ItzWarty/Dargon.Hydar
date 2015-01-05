using System;
using Dargon.Hydar.Clustering;

namespace Dargon.Hydar.Peering {
   public interface PeerStatusFactory {
      ManageablePeerStatus Create(Guid guid);
   }

   public class PeerStatusFactoryImpl : PeerStatusFactory {
      private readonly PeeringConfiguration peeringConfiguration;

      public PeerStatusFactoryImpl(PeeringConfigurationImpl peeringConfiguration) {
         this.peeringConfiguration = peeringConfiguration;
      }

      public ManageablePeerStatus Create(Guid guid) {
         return new PeerStatusImpl(peeringConfiguration, guid);
      }
   }
}
