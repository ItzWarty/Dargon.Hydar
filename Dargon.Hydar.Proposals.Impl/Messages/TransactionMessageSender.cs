using Dargon.Hydar.Caching.Data.Operations;
using Dargon.Hydar.Networking;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Networking.Utilities;
using Dargon.Hydar.Proposals.Messages.Helpers;
using System;

namespace Dargon.Hydar.Proposals.Messages {
   public interface TransactionMessageSender<K, V> {
      void LeaderPrepare(Guid proposalId, K entryKey, EntryOperation operation);
      void LeaderCommit(Guid proposalId);
      void LeaderCancel(Guid proposalId);
      void FollowerAccept(Guid proposalId);
      void FollowerReject(Guid proposalId, RejectionReason rejectionReason);
   }

   public class TransactionMessageSenderImpl<K, V> : MessageSenderBase, TransactionMessageSender<K, V> {
      private readonly Guid cacheId;
      private readonly TransactionMessageFactory<K, V> transactionMessageFactory;

      public TransactionMessageSenderImpl(
         Guid cacheId,
         OutboundEnvelopeFactory outboundEnvelopeFactory,
         OutboundEnvelopeBus outboundEnvelopeBus,
         TransactionMessageFactory<K, V> transactionMessageFactory
      ) : base(outboundEnvelopeFactory, outboundEnvelopeBus) {
         this.cacheId = cacheId;
         this.transactionMessageFactory = transactionMessageFactory;
      }

      public void LeaderPrepare(Guid proposalId, K entryKey, EntryOperation operation) {
         SendMessageMulticast(cacheId, transactionMessageFactory.LeaderPrepare(proposalId, entryKey, operation));
      }

      public void LeaderCommit(Guid proposalId) {
         SendMessageMulticast(cacheId, transactionMessageFactory.LeaderCommit(proposalId));
      }

      public void LeaderCancel(Guid proposalId) {
         SendMessageMulticast(cacheId, transactionMessageFactory.LeaderCancel(proposalId));
      }

      public void FollowerAccept(Guid proposalId) {
         SendMessageMulticast(cacheId, transactionMessageFactory.FollowerAccept(proposalId));
      }

      public void FollowerReject(Guid proposalId, RejectionReason rejectionReason) {
         SendMessageMulticast(cacheId, transactionMessageFactory.FollowerReject(proposalId, rejectionReason));
      }
   }
}
