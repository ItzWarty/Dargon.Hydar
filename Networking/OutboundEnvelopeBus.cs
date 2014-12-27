using Dargon.Audits;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Networking {
   public interface OutboundEnvelopeBus : EventBus<OutboundEnvelope> { }

   public class OutboundEnvelopeBusImpl : OutboundEnvelopeBus {
      public void Post(OutboundEnvelope obj) {
         var capture = EventPosted;
         if (capture != null) {
            capture(this, obj);
         }
      }

      public event EventBusHandler<OutboundEnvelope> EventPosted;
   }
}
