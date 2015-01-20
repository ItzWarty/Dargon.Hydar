using Dargon.Hydar.Networking;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Networking.Utilities;
using Dargon.Hydar.Proposals.Messages;
using ItzWarty.Collections;
using ItzWarty.Threading;
using System;

namespace Dargon.Hydar.Proposals {
   public interface AtomicProposalEnvelopeChannelConsumer<TSubject> {

   }

   public class AtomicProposalEnvelopeChannelConsumerImpl<TSubject> : EnvelopeProcessorBase<InboundEnvelope, Action<InboundEnvelope>>, AtomicProposalEnvelopeChannelConsumer<TSubject> {
      private readonly IThreadingProxy threadingProxy;
      private readonly HydarIdentity hydarIdentity;
      private readonly InboundEnvelopeChannel inboundEnvelopeChannel;
      private readonly ProposalStateManager<TSubject> proposalStateManager;
      private readonly Guid cacheId;
      private readonly IThread processorThread;
      private readonly ICancellationTokenSource processorCancellationTokenSource;

      public AtomicProposalEnvelopeChannelConsumerImpl(IThreadingProxy threadingProxy, HydarIdentity hydarIdentity, InboundEnvelopeChannel inboundEnvelopeChannel, ProposalStateManager<TSubject> proposalStateManager, Guid cacheId) {
         this.threadingProxy = threadingProxy;
         this.hydarIdentity = hydarIdentity;
         this.inboundEnvelopeChannel = inboundEnvelopeChannel;
         this.proposalStateManager = proposalStateManager;
         this.cacheId = cacheId;

         this.processorThread = threadingProxy.CreateThread(ProcessingThreadStart, new ThreadCreationOptions { IsBackground = true });
         this.processorCancellationTokenSource = threadingProxy.CreateCancellationTokenSource();
      }

      public void Initialize() {
         RegisterHandler<AtomicProposalPrepareImpl<TSubject>>(HandleProposalPrepare);
         RegisterHandler<AtomicProposalCommitImpl>(HandleProposalCommit);
         RegisterHandler<AtomicProposalCancelImpl>(HandleProposalCancel);
         RegisterHandler<AtomicProposalAcceptImpl>(HandleProposalAccept);
         RegisterHandler<AtomicProposalRejectImpl>(HandleProposalReject);

         processorThread.Start();
      }

      private void ProcessingThreadStart() {
         while (!processorCancellationTokenSource.IsCancellationRequested) {
            var envelope = inboundEnvelopeChannel.TakeEnvelope(processorCancellationTokenSource.Token);
            var header = envelope.Header;
            var recipientId = header.RecipientId;
            if (recipientId == cacheId) {
               var proposalMessage = envelope.Message as AtomicProposalMessage;
               if (proposalMessage != null) {
                  Process(envelope);
               }
            }
         }
      }

      private void HandleProposalPrepare(InboundEnvelope envelope) {
         var message = (AtomicProposalPrepare<TSubject>)envelope.Message;
         proposalStateManager.HandleProposalPrepare(message.ProposalId, message.Proposal);
      }

      private void HandleProposalCommit(InboundEnvelope envelope) {
         var message = (AtomicProposalMessage)envelope.Message;
         var proposalState = proposalStateManager.GetProposalStateByIdOrNull(message.ProposalId);
         if (proposalState != null) {
            proposalState.HandleCommit();
         }
      }

      private void HandleProposalCancel(InboundEnvelope envelope) {
         var message = (AtomicProposalMessage)envelope.Message;
         var proposalState = proposalStateManager.GetProposalStateByIdOrNull(message.ProposalId);
         if (proposalState != null) {
            proposalState.HandleCancel();
         }
      }

      private void HandleProposalAccept(InboundEnvelope envelope) {
         var message = (AtomicProposalMessage)envelope.Message;
         var proposalState = proposalStateManager.GetProposalStateByIdOrNull(message.ProposalId);
         if (proposalState != null) {
            proposalState.HandleAccept(envelope.Header.SenderId);
         }
      }

      private void HandleProposalReject(InboundEnvelope envelope) {
         var message = (AtomicProposalReject)envelope.Message;
         var proposalState = proposalStateManager.GetProposalStateByIdOrNull(message.ProposalId);
         if (proposalState != null) {
            proposalState.HandleReject(envelope.Header.SenderId, message.RejectionReason);
         }
      }

      protected override void Invoke(Action<InboundEnvelope> handler, InboundEnvelope envelope) {
         handler(envelope);
      }
   }
}