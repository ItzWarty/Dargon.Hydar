using Dargon.Hydar.Clustering.Messages.Helpers;
using Dargon.Hydar.Networking;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Utilities;
using Dargon.PortableObjects;
using System;

namespace Dargon.Hydar.Clustering.Messages {
   public interface ClusteringMessageSender {
      void ElectionVote(ElectionCandidate candidate);
      void ElectionAcknowledgement(Guid acknowledgedVoter);
      void EpochLeaderHeartBeat(DateTimeInterval epochInterval, EpochSummary currentEpochSummary, EpochSummary previousEpochSummary);
   }

   public class ClusteringMessageSenderImpl : ClusteringMessageSender {
      private readonly OutboundEnvelopeFactory outboundEnvelopeFactory;
      private readonly OutboundEnvelopeBus outboundEnvelopeBus;
      private readonly ClusteringMessageFactory clusteringMessageFactory;

      public ClusteringMessageSenderImpl(OutboundEnvelopeFactory outboundEnvelopeFactory, OutboundEnvelopeBus outboundEnvelopeBus, ClusteringMessageFactory clusteringMessageFactory) {
         this.outboundEnvelopeFactory = outboundEnvelopeFactory;
         this.outboundEnvelopeBus = outboundEnvelopeBus;
         this.clusteringMessageFactory = clusteringMessageFactory;
      }

      public void ElectionVote(ElectionCandidate candidate) {
         SendMessageBroadcast(clusteringMessageFactory.ElectionVote(candidate));
      }

      public void ElectionAcknowledgement(Guid acknowledgedVoter) {
         SendMessageUnicast(acknowledgedVoter, clusteringMessageFactory.ElectionAcknowledgement(acknowledgedVoter));
      }

      public void EpochLeaderHeartBeat(DateTimeInterval epochInterval, EpochSummary currentEpochSummary, EpochSummary previousEpochSummary) {
         SendMessageBroadcast(clusteringMessageFactory.EpochLeaderHeartBeat(epochInterval, currentEpochSummary, previousEpochSummary));
      }

      internal void SendMessageUnicast<TMessage>(Guid recipientId, TMessage message) {
         var envelope = outboundEnvelopeFactory.CreateUnicastEnvelope(recipientId, message);
         outboundEnvelopeBus.Post(envelope);
      }

      internal void SendMessageBroadcast<TMessage>(TMessage message) {
         var envelope = outboundEnvelopeFactory.CreateBroadcastEnvelope(message);
         outboundEnvelopeBus.Post(envelope);
      }
   }
}
