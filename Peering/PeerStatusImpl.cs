using Dargon.Hydar.Peering.Messages;
using ItzWarty;
using System;

namespace Dargon.Hydar.Peering {
   public class PeerStatusImpl : ManageablePeerStatus {
      private readonly object synchronization = new object();
      private readonly PeeringConfiguration peeringConfiguration;
      private readonly Guid id;
      private uint address;
      private long lastHeartBeatTime = -1;

      public PeerStatusImpl(PeeringConfiguration peeringConfiguration, Guid id) {
         this.peeringConfiguration = peeringConfiguration;
         this.id = id;
      }

      public Guid Id { get { lock (synchronization) return id; } }
      public long LastHeartBeatTime { get { lock (synchronization) return lastHeartBeatTime; } }
      public bool IsActive { get { lock (synchronization) return Util.GetUnixTimeMilliseconds() < lastHeartBeatTime + peeringConfiguration.MaximumMissedHeartBeatIntervalToInactivity; } }

      public void Update(uint newAddress, PeeringAnnounce peeringAnnounce) {
         lock (synchronization) {
            address = newAddress;
            lastHeartBeatTime = Util.GetUnixTimeMilliseconds();
         }
      }
   }
}