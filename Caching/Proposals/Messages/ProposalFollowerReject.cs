using System;
using Dargon.Hydar.Caching.Proposals.Messages.Helpers;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Caching.Proposals.Messages {
   public class ProposalFollowerReject : IPortableObject, IProposalMessage {
      private Guid proposalId;
      private RejectionReason rejectionReason;

      public ProposalFollowerReject(Guid proposalId, RejectionReason rejectionReason) {
         this.proposalId = proposalId;
         this.rejectionReason = rejectionReason;
      }

      public Guid ProposalId { get { return proposalId; } }
      public RejectionReason RejectionReason {  get { return rejectionReason; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, proposalId);
         writer.WriteS32(1, (int)rejectionReason);
      }

      public void Deserialize(IPofReader reader) {
         proposalId = reader.ReadGuid(0);
         rejectionReason = (RejectionReason)reader.ReadS32(1);
      }
   }
}
