using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;
using System;
using Dargon.Hydar.Caching.Proposals;
using Dargon.Hydar.Caching.Proposals.Messages;

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
      private readonly ProposalManager proposalManager;

      public CacheDispatcherImpl(string cacheName, Guid cacheIdentifier, HydarIdentity nodeIdentity, ProposalManager proposalManager) {
         this.cacheName = cacheName;
         this.cacheIdentifier = cacheIdentifier;
         this.nodeIdentity = nodeIdentity;
         this.proposalManager = proposalManager;
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
         proposalManager.Process(envelope);
      }

      protected override void Invoke(Action<InboundEnvelope> handler, InboundEnvelope envelope) {
         handler(envelope);
      }

      public void Dispatch(InboundEnvelope e) {
         Process(e);
      }
   }
}
