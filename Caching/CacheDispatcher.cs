using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;
using System;
using Dargon.Hydar.Networking;
using Dargon.Hydar.Proposals;
using Dargon.Hydar.Proposals.Messages;

namespace Dargon.Hydar.Caching {
   public interface CacheDispatcher {
      string Name { get; }
      Guid Id { get; }

      void Dispatch(InboundEnvelope e);
   }

   public class CacheDispatcherImpl<K, V> : EnvelopeProcessorBase<InboundEnvelope, Action<InboundEnvelope>>, CacheDispatcher {
      private readonly string cacheName;
      private readonly Guid cacheIdentifier;
      private readonly HydarIdentity nodeIdentity;
      private readonly InboundEnvelopeChannel topicEnvelopeChannel;

      public CacheDispatcherImpl(string cacheName, Guid cacheIdentifier, HydarIdentity nodeIdentity, InboundEnvelopeChannel topicEnvelopeChannel) {
         this.cacheName = cacheName;
         this.cacheIdentifier = cacheIdentifier;
         this.nodeIdentity = nodeIdentity;
         this.topicEnvelopeChannel = topicEnvelopeChannel;
      }

      public string Name { get { return cacheName; } }
      public Guid Id { get { return cacheIdentifier; } }

      public void Initialize() {
         nodeIdentity.AddGroup(cacheIdentifier, cacheName);

         RegisterHandler<ProposalLeaderPrepare<K>>(RouteToProposalManager);
         RegisterHandler<ProposalFollowerAccept>(RouteToProposalManager);
         RegisterHandler<ProposalFollowerReject>(RouteToProposalManager);
         RegisterHandler<ProposalLeaderCommit>(RouteToProposalManager);
         RegisterHandler<ProposalLeaderCancel>(RouteToProposalManager);
      }

      private void RouteToProposalManager(InboundEnvelope envelope) {
         topicEnvelopeChannel.PostEnvelope(envelope);
      }

      protected override void Invoke(Action<InboundEnvelope> handler, InboundEnvelope envelope) {
         handler(envelope);
      }

      public void Dispatch(InboundEnvelope e) {
         Process(e);
      }
   }
}
