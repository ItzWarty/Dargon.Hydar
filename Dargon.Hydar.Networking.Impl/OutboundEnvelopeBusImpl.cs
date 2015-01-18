using Dargon.Audits;
using Dargon.Hydar.Networking.PortableObjects;

namespace Dargon.Hydar.Networking {
   public class OutboundEnvelopeBusImpl : OutboundEnvelopeBus {
      private object x = new object();
      public void Post(OutboundEnvelope obj) {
         lock (x) {
            var capture = EventPosted;
            if (capture != null) {
               capture(this, obj);
            }
         }
      }

      public event EventBusHandler<OutboundEnvelope> EventPosted;
   }
}