using Dargon.Hydar.Networking;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Networking.Utilities;
using Dargon.Hydar.Proposals.Messages;
using ItzWarty.Collections;
using ItzWarty.Threading;
using System;

namespace Dargon.Hydar.Proposals {
   public interface TopicEnvelopeDispatcher<K, V> {

   }

   public class TopicEnvelopeDispatcherImpl<K, V> : EnvelopeProcessorBase<InboundEnvelope, Action<InboundEnvelope>>, TopicEnvelopeDispatcher<K, V> {
      private readonly IConcurrentDictionary<Guid, ProposalContext<K, V>> proposalContextsById = new ConcurrentDictionary<Guid, ProposalContext<K, V>>();
      private readonly IThreadingProxy threadingProxy;
      private readonly HydarIdentity hydarIdentity;
      private readonly InboundEnvelopeChannel inboundEnvelopeChannel;
      private readonly ProposalContextFactory<K, V> proposalContextFactory;
      private readonly Guid cacheId;
      private readonly IThread processorThread;
      private readonly ICancellationTokenSource processorCancellationTokenSource;

      public TopicEnvelopeDispatcherImpl(IThreadingProxy threadingProxy, HydarIdentity hydarIdentity, InboundEnvelopeChannel inboundEnvelopeChannel, ProposalContextFactory<K, V> proposalContextFactory, Guid cacheId) {
         this.threadingProxy = threadingProxy;
         this.hydarIdentity = hydarIdentity;
         this.inboundEnvelopeChannel = inboundEnvelopeChannel;
         this.proposalContextFactory = proposalContextFactory;
         this.cacheId = cacheId;

         this.processorThread = threadingProxy.CreateThread(ProcessingThreadStart, new ThreadCreationOptions { IsBackground = true });
         this.processorCancellationTokenSource = threadingProxy.CreateCancellationTokenSource();
      }

      public void Initialize() {
         RegisterHandler<ProposalLeaderPrepare<K>>(HandleProposalPrepare);
         RegisterHandler<ProposalLeaderCommit>(HandleProposalResponse);
         RegisterHandler<ProposalLeaderCancel>(HandleProposalResponse);
         RegisterHandler<ProposalFollowerAccept>(HandleProposalResponse);
         RegisterHandler<ProposalFollowerReject>(HandleProposalResponse);

         processorThread.Start();
      }

      private void ProcessingThreadStart() {
         while (!processorCancellationTokenSource.IsCancellationRequested) {
            var envelope = inboundEnvelopeChannel.TakeEnvelope(processorCancellationTokenSource.Token);
            var header = envelope.Header;
            var recipientId = header.RecipientId;
            if (recipientId == Guid.Empty || recipientId == hydarIdentity.NodeId || recipientId == cacheId) {
               var proposalMessage = envelope.Message as IProposalMessage;
               if (proposalMessage != null && proposalMessage.TopicId == cacheId) {
                  Process(envelope);
               }
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