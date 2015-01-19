using System;
using Dargon.Hydar.Caching.Data.Operations;
using Dargon.Hydar.Proposals.Messages.Helpers;

namespace Dargon.Hydar.Proposals.Messages {
   public interface TransactionMessageFactory<K, V> {
      AtomicProposalPrepareImpl<K> LeaderPrepare(Guid proposalId, K entryKey, EntryOperation operation);
      AtomicProposalCommitImpl LeaderCommit(Guid proposalId);
      AtomicProposalCancelImpl LeaderCancel(Guid proposalId);
      AtomicProposalAcceptImpl FollowerAccept(Guid proposalId);
      AtomicProposalRejectImpl FollowerReject(Guid proposalId, RejectionReason rejectionReason);
   }

   public class TransactionMessageFactoryImpl<K, V> : TransactionMessageFactory<K, V> {
      private readonly Guid cacheId;

      public TransactionMessageFactoryImpl(Guid cacheId) {
         this.cacheId = cacheId;
      }

      public AtomicProposalPrepareImpl<K> LeaderPrepare(Guid proposalId, K entryKey, EntryOperation operation) {
         return new AtomicProposalPrepareImpl<K>(cacheId, proposalId, entryKey, operation);
      }

      public AtomicProposalCommitImpl LeaderCommit(Guid proposalId) {
         return new AtomicProposalCommitImpl(cacheId, proposalId);
      }

      public AtomicProposalCancelImpl LeaderCancel(Guid proposalId) {
         return new AtomicProposalCancelImpl(cacheId, proposalId);
      }

      public AtomicProposalAcceptImpl FollowerAccept(Guid proposalId) {
         return new AtomicProposalAcceptImpl(cacheId, proposalId);
      }

      public AtomicProposalRejectImpl FollowerReject(Guid proposalId, RejectionReason rejectionReason) {
         return new AtomicProposalRejectImpl(cacheId, proposalId, rejectionReason);
      }
   }
}