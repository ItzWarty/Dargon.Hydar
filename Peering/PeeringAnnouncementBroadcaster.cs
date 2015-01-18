using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Peering.Messages;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Peering {
   public interface PeeringAnnouncementBroadcaster {
   }

   public class PeeringAnnouncementBroadcasterImpl : PeeringAnnouncementBroadcaster {
      private readonly PeriodicTicker hydarPeriodicTicker;
      private readonly PeeringMessageSender peeringMessageSender;

      public PeeringAnnouncementBroadcasterImpl(PeriodicTicker hydarPeriodicTicker, PeeringMessageSender peeringMessageSender) {
         this.hydarPeriodicTicker = hydarPeriodicTicker;
         this.peeringMessageSender = peeringMessageSender;
      }

      public void Initialize() {
         hydarPeriodicTicker.Tick += (s, e) => Tick();
      }

      internal void Tick() {
         peeringMessageSender.PeeringAnnounce();
      }
   }
}
