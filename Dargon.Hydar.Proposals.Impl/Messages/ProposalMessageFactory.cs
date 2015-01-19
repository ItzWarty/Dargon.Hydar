using System;
using Dargon.Hydar.Caching.Data.Operations;
using Dargon.Hydar.Proposals.Messages.Helpers;

namespace Dargon.Hydar.Proposals.Messages {
   public interface ProposalMessageFactory<K, V> {
      ProposalLeaderPrepare<K> LeaderPrepare(Guid proposalId, K entryKey, EntryOperation operation);
      ProposalLeaderCommit LeaderCommit(Guid proposalId);
      ProposalLeaderCancel LeaderCancel(Guid proposalId);
      ProposalFollowerAccept FollowerAccept(Guid proposalId);
      ProposalFollowerReject FollowerReject(Guid proposalId, RejectionReason rejectionReason);
   }

   public class ProposalMessageFactoryImpl<K, V> : ProposalMessageFactory<K, V> {
      private readonly Guid cacheId;

      public ProposalMessageFactoryImpl(Guid cacheId) {
         this.cacheId = cacheId;
      }

      public ProposalLeaderPrepare<K> LeaderPrepare(Guid proposalId, K entryKey, EntryOperation operation) {
         return new ProposalLeaderPrepare<K>(cacheId, proposalId, entryKey, operation);
      }

      public ProposalLeaderCommit LeaderCommit(Guid proposalId) {
         return new ProposalLeaderCommit(cacheId, proposalId);
      }

      public ProposalLeaderCancel LeaderCancel(Guid proposalId) {
         return new ProposalLeaderCancel(cacheId, proposalId);
      }

      public ProposalFollowerAccept FollowerAccept(Guid proposalId) {
         return new ProposalFollowerAccept(cacheId, proposalId);
      }

      public ProposalFollowerReject FollowerReject(Guid proposalId, RejectionReason rejectionReason) {
         return new ProposalFollowerReject(cacheId, proposalId, rejectionReason);
      }
   }
}