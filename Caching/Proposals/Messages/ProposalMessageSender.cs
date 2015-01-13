using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Caching.Data.Operations;
using Dargon.Hydar.Caching.Proposals.Messages.Helpers;
using Dargon.Hydar.Networking;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Caching.Proposals.Messages {
   public interface ProposalMessageSender<K, V> {
      void LeaderPrepare(Guid proposalId, K entryKey, EntryOperation operation);
      void LeaderCommit(Guid proposalId);
      void LeaderCancel(Guid proposalId);
      void FollowerAccept(Guid proposalId);
      void FollowerReject(Guid proposalId, RejectionReason rejectionReason);
   }

   public class ProposalMessageSenderImpl<K, V> : MessageSenderBase, ProposalMessageSender<K, V> {
      private readonly Guid cacheId;
      private readonly ProposalMessageFactory<K, V> proposalMessageFactory;

      public ProposalMessageSenderImpl(
         Guid cacheId,
         OutboundEnvelopeFactory outboundEnvelopeFactory,
         OutboundEnvelopeBus outboundEnvelopeBus,
         ProposalMessageFactory<K, V> proposalMessageFactory
      ) : base(outboundEnvelopeFactory, outboundEnvelopeBus) {
         this.cacheId = cacheId;
         this.proposalMessageFactory = proposalMessageFactory;
      }

      public void LeaderPrepare(Guid proposalId, K entryKey, EntryOperation operation) {
         SendMessageMulticast(cacheId, proposalMessageFactory.LeaderPrepare(proposalId, entryKey, operation));
      }

      public void LeaderCommit(Guid proposalId) {
         SendMessageMulticast(cacheId, proposalMessageFactory.LeaderCommit(proposalId));
      }

      public void LeaderCancel(Guid proposalId) {
         SendMessageMulticast(cacheId, proposalMessageFactory.LeaderCancel(proposalId));
      }

      public void FollowerAccept(Guid proposalId) {
         SendMessageMulticast(cacheId, proposalMessageFactory.FollowerAccept(proposalId));
      }

      public void FollowerReject(Guid proposalId, RejectionReason rejectionReason) {
         SendMessageMulticast(cacheId, proposalMessageFactory.FollowerReject(proposalId, rejectionReason));
      }
   }
}
