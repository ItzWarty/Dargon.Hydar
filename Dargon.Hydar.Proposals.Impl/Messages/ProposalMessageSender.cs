using Dargon.Hydar.Networking;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Networking.Utilities;
using Dargon.Hydar.Proposals.Messages.Helpers;
using System;
using System.Runtime;

namespace Dargon.Hydar.Proposals.Messages {
   public interface ProposalMessageSender<TSubject> {
      void LeaderPrepare(Guid proposalId, TSubject entryKey, Proposal<TSubject> proposal);
      void LeaderCommit(Guid proposalId);
      void LeaderCancel(Guid proposalId);
      void FollowerAccept(Guid proposalId);
      void FollowerReject(Guid proposalId, RejectionReason rejectionReason);
      void FollowerCommitAcknowledgement(Guid proposalId);
      void FollowerCancelAcknowledgement(Guid proposalId);
   }

   public class ProposalMessageSenderImpl<TSubject> : MessageSenderBase, ProposalMessageSender<TSubject> {
      private readonly Guid cacheId;
      private readonly AtomicProposalMessageFactory<TSubject> atomicProposalMessageFactory;

      public ProposalMessageSenderImpl(
         Guid cacheId,
         OutboundEnvelopeFactory outboundEnvelopeFactory,
         OutboundEnvelopeBus outboundEnvelopeBus,
         AtomicProposalMessageFactory<TSubject> atomicProposalMessageFactory
      ) : base(outboundEnvelopeFactory, outboundEnvelopeBus) {
         this.cacheId = cacheId;
         this.atomicProposalMessageFactory = atomicProposalMessageFactory;
      }

      public void LeaderPrepare(Guid proposalId, TSubject entryKey, Proposal<TSubject> proposal) {
         SendMessageMulticast(cacheId, atomicProposalMessageFactory.LeaderPrepare(proposalId, entryKey, proposal));
      }

      public void LeaderCommit(Guid proposalId) {
         SendMessageMulticast(cacheId, atomicProposalMessageFactory.LeaderCommit(proposalId));
      }

      public void LeaderCancel(Guid proposalId) {
         SendMessageMulticast(cacheId, atomicProposalMessageFactory.LeaderCancel(proposalId));
      }

      public void FollowerAccept(Guid proposalId) {
         SendMessageMulticast(cacheId, atomicProposalMessageFactory.FollowerAccept(proposalId));
      }

      public void FollowerReject(Guid proposalId, RejectionReason rejectionReason) {
         SendMessageMulticast(cacheId, atomicProposalMessageFactory.FollowerReject(proposalId, rejectionReason));
      }

      public void FollowerCommitAcknowledgement(Guid proposalId) {
         SendMessageMulticast(cacheId, atomicProposalMessageFactory.FollowerCommitAcknowledgement(proposalId));
      }

      public void FollowerCancelAcknowledgement(Guid proposalId) {
         SendMessageMulticast(cacheId, atomicProposalMessageFactory.FollowerCancelAcknowledgement(proposalId));
      }
   }
}
