using Dargon.Audits;
using Dargon.Hydar.PortableObjects;
using ItzWarty.Collections;
using ItzWarty.Threading;
using System;

namespace Dargon.Hydar.Networking {
   public interface InboundEnvelopeChannel : IDisposable {
      InboundEnvelope TakeEnvelope(ICancellationToken cancellationToken);
   }

   public class InboundBusToInboundEnvelopeChannel : InboundEnvelopeChannel {
      private readonly IThreadingProxy threadingProxy;
      private readonly InboundEnvelopeBus inboundEnvelopeBus;
      private readonly ICancellationTokenSource cancellationTokenSource;
      private readonly IConcurrentQueue<InboundEnvelope> inboundEnvelopeQueue = new ConcurrentQueue<InboundEnvelope>();
      private readonly ISemaphore inboundEnvelopeCounter = new SemaphoreProxy(0, int.MaxValue);

      public InboundBusToInboundEnvelopeChannel(IThreadingProxy threadingProxy, InboundEnvelopeBus inboundEnvelopeBus) {
         this.threadingProxy = threadingProxy;
         this.inboundEnvelopeBus = inboundEnvelopeBus;
         this.cancellationTokenSource = threadingProxy.CreateCancellationTokenSource();
      }

      public void Initialize() {
         inboundEnvelopeBus.EventPosted += HandleEnvelopePosted;
      }

      private void HandleEnvelopePosted(EventBus<InboundEnvelope> sender, InboundEnvelope e) {
         inboundEnvelopeQueue.Enqueue(e);
         inboundEnvelopeCounter.Release();
      }

      public InboundEnvelope TakeEnvelope(ICancellationToken inputCancellationToken) {
         using (var tokenSource = threadingProxy.CreateLinkedCancellationTokenSource(cancellationTokenSource.Token, inputCancellationToken)) {
            inboundEnvelopeCounter.Wait(tokenSource.Token);
            InboundEnvelope envelope;
            while (!inboundEnvelopeQueue.TryDequeue(out envelope)) ;
            return envelope;
         }
      }

      public void Dispose() {
         cancellationTokenSource.Cancel();
         cancellationTokenSource.Dispose();
         inboundEnvelopeCounter.Release();
      }
   }
}
