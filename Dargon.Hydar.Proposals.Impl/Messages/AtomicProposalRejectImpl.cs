using System;
using Dargon.Hydar.Proposals.Messages.Helpers;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Proposals.Messages {
   public class AtomicProposalRejectImpl : IPortableObject, AtomicProposalMessage {
      private Guid topicId;
      private Guid proposalId;
      private RejectionReason rejectionReason;

      public AtomicProposalRejectImpl(Guid topicId, Guid proposalId, RejectionReason rejectionReason) {
         this.topicId = topicId;
         this.proposalId = proposalId;
         this.rejectionReason = rejectionReason;
      }

      public Guid TopicId {  get { return topicId; } }
      public Guid ProposalId { get { return proposalId; } }
      public RejectionReason RejectionReason {  get { return rejectionReason; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, topicId);
         writer.WriteGuid(1, proposalId);
         writer.WriteS32(2, (int)rejectionReason);
      }

      public void Deserialize(IPofReader reader) {
         topicId = reader.ReadGuid(0);
         proposalId = reader.ReadGuid(1);
         rejectionReason = (RejectionReason)reader.ReadS32(2);
      }
   }
}
