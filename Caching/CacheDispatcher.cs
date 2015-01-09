using Dargon.Hydar.Caching.Management;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;
using System;

namespace Dargon.Hydar.Caching {
   public interface CacheDispatcher {
      string Name { get; }
      Guid Id { get; }

      void Dispatch(InboundEnvelope e);
   }

   public class CacheDispatcherImpl : EnvelopeProcessorBase<InboundEnvelope, Action<InboundEnvelope>>, CacheDispatcher {
      private readonly string cacheName;
      private readonly Guid cacheIdentifier;
      private readonly HydarIdentity nodeIdentity;

      public CacheDispatcherImpl(string cacheName, Guid cacheIdentifier, HydarIdentity nodeIdentity) {
         this.cacheName = cacheName;
         this.cacheIdentifier = cacheIdentifier;
         this.nodeIdentity = nodeIdentity;
      }

      public string Name { get { return cacheName; } }
      public Guid Id { get { return cacheIdentifier; } }

      public void Initialize() {
         nodeIdentity.AddGroup(cacheIdentifier, cacheName);

         RegisterHandler<ProposalCommit>(DispatchToProposalContext);
      }

      private void DispatchToProposalContext(InboundEnvelope obj) {

      }

      protected override void Invoke(Action<InboundEnvelope> handler, InboundEnvelope envelope) {
         handler(envelope);
      }

      public void Dispatch(InboundEnvelope e) {
         Process(e);
      }
   }
}
