using Dargon.Audits;
using Dargon.Hydar.Networking.PortableObjects;

namespace Dargon.Hydar.Networking {
   public class InboundEnvelopeBusImpl : InboundEnvelopeBus {
      public void Post(InboundEnvelope obj) {
         var capture = EventPosted;
         if (capture != null) {
            capture(this, obj);
         }
      }

      public event EventBusHandler<InboundEnvelope> EventPosted;
   }
}