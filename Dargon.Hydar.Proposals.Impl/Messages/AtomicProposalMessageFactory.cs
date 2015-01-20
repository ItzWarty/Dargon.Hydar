using System;
using Dargon.Hydar.Proposals.Messages.Helpers;

namespace Dargon.Hydar.Proposals.Messages {
   public interface AtomicProposalMessageFactory<TSubject> {
      AtomicProposalPrepareImpl<TSubject> LeaderPrepare(Guid proposalId, TSubject subject, Proposal<TSubject> propsal);
      AtomicProposalCommitImpl LeaderCommit(Guid proposalId);
      AtomicProposalCancelImpl LeaderCancel(Guid proposalId);
      AtomicProposalAcceptImpl FollowerAccept(Guid proposalId);
      AtomicProposalRejectImpl FollowerReject(Guid proposalId, RejectionReason rejectionReason);
      AtomicProposalCommitAcknowledgementImpl FollowerCommitAcknowledgement(Guid proposalId);
      AtomicProposalCancelAcknowledgementImpl FollowerCancelAcknowledgement(Guid proposalId);
   }

   public class AtomicProposalMessageFactoryImpl<TSubject> : AtomicProposalMessageFactory<TSubject> {
      private readonly Guid cacheId;

      public AtomicProposalMessageFactoryImpl(Guid cacheId) {
         this.cacheId = cacheId;
      }

      public AtomicProposalPrepareImpl<TSubject> LeaderPrepare(Guid proposalId, TSubject entryKey, Proposal<TSubject> propsal) {
         return new AtomicProposalPrepareImpl<TSubject>(cacheId, proposalId, entryKey, propsal);
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

      public AtomicProposalCommitAcknowledgementImpl FollowerCommitAcknowledgement(Guid proposalId) {
         return new AtomicProposalCommitAcknowledgementImpl(cacheId, proposalId);
      }

      public AtomicProposalCancelAcknowledgementImpl FollowerCancelAcknowledgement(Guid proposalId) {
         return new AtomicProposalCancelAcknowledgementImpl(cacheId, proposalId);
      }
   }
}