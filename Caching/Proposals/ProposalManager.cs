using System;
using Dargon.Audits;
using Dargon.Hydar.Caching.Data.Operations;
using Dargon.Hydar.Caching.Proposals.Messages;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Caching.Proposals {
   public interface ProposalManager<K, V> {
      bool Process(InboundEnvelope envelope);
   }

   public class ProposalManagerImpl<K, V> : EnvelopeProcessorBase<InboundEnvelope, Action<InboundEnvelope>>, ProposalManager<K, V> {
      private readonly IConcurrentDictionary<Guid, ProposalContext<K, V>> proposalContextsById = new ConcurrentDictionary<Guid, ProposalContext<K, V>>();
      private readonly HydarIdentity hydarIdentity;
      private readonly InboundEnvelopeBus inboundEnvelopeBus;
      private readonly ProposalContextFactory<K, V> proposalContextFactory;
      private readonly Guid cacheId;

      public ProposalManagerImpl(HydarIdentity hydarIdentity, InboundEnvelopeBus inboundEnvelopeBus, ProposalContextFactory<K, V> proposalContextFactory, Guid cacheId) {
         this.hydarIdentity = hydarIdentity;
         this.inboundEnvelopeBus = inboundEnvelopeBus;
         this.proposalContextFactory = proposalContextFactory;
         this.cacheId = cacheId;
      }

      public void Initialize() {
         inboundEnvelopeBus.EventPosted += HandleInboundEnvelope;

         RegisterHandler<ProposalLeaderPrepare<K>>(HandleProposalPrepare);
         RegisterHandler<ProposalLeaderCommit>(HandleProposalResponse);
         RegisterHandler<ProposalLeaderCancel>(HandleProposalResponse);
         RegisterHandler<ProposalFollowerAccept>(HandleProposalResponse);
         RegisterHandler<ProposalFollowerReject>(HandleProposalResponse);
      }

      private void HandleInboundEnvelope(EventBus<InboundEnvelope> sender, InboundEnvelope e) {
         var header = e.Header;
         var recipientId = header.RecipientId;
         if (recipientId == Guid.Empty || recipientId == hydarIdentity.NodeId || recipientId == cacheId) {
            var proposalMessage = e.Message as IProposalMessage;
            if (proposalMessage.CacheId == cacheId) {
               Process(e);
            }
         }
      }

      private void HandleProposalPrepare(InboundEnvelope envelope) {
         var message = (ProposalLeaderPrepare<K>)envelope.Message;
         var proposal = proposalContextsById.GetOrAdd(
            message.ProposalId,
            guid => proposalContextFactory.Create(message)
         );
         proposal.Process(envelope);
      }

      private void HandleProposalResponse(InboundEnvelope envelope) {
         var message = (IProposalMessage)envelope.Message;
         ProposalContext<K, V> proposalContext;
         if (proposalContextsById.TryGetValue(message.ProposalId, out proposalContext)) {
            proposalContext.Process(envelope);
         }
      }

      protected override void Invoke(Action<InboundEnvelope> handler, InboundEnvelope envelope) {
         handler(envelope);
      }
   }
}