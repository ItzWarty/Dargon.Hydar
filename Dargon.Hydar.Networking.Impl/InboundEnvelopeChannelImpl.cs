using Dargon.Hydar.Networking.PortableObjects;
using ItzWarty.Collections;
using ItzWarty.Threading;

namespace Dargon.Hydar.Networking {
   public class InboundEnvelopeChannelImpl : InboundEnvelopeChannel {
      private readonly IThreadingProxy threadingProxy;
      private readonly ICancellationTokenSource cancellationTokenSource;
      private readonly ISemaphore inboundEnvelopeCounter;
      private readonly IConcurrentQueue<InboundEnvelope> inboundEnvelopeQueue = new ConcurrentQueue<InboundEnvelope>();

      public InboundEnvelopeChannelImpl(IThreadingProxy threadingProxy) {
         this.threadingProxy = threadingProxy;
         this.cancellationTokenSource = threadingProxy.CreateCancellationTokenSource();
         this.inboundEnvelopeCounter = threadingProxy.CreateSemaphore(0, int.MaxValue);
      }

      public void PostEnvelope(InboundEnvelope e) {
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