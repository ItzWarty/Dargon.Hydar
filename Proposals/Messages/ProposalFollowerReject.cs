using System;
using Dargon.Hydar.Proposals.Messages.Helpers;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Proposals.Messages {
   public class ProposalFollowerReject : IPortableObject, IProposalMessage {
      private Guid cacheId;
      private Guid proposalId;
      private RejectionReason rejectionReason;

      public ProposalFollowerReject(Guid cacheId, Guid proposalId, RejectionReason rejectionReason) {
         this.cacheId = cacheId;
         this.proposalId = proposalId;
         this.rejectionReason = rejectionReason;
      }

      public Guid CacheId {  get { return cacheId; } }
      public Guid ProposalId { get { return proposalId; } }
      public RejectionReason RejectionReason {  get { return rejectionReason; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, cacheId);
         writer.WriteGuid(1, proposalId);
         writer.WriteS32(2, (int)rejectionReason);
      }

      public void Deserialize(IPofReader reader) {
         cacheId = reader.ReadGuid(0);
         proposalId = reader.ReadGuid(1);
         rejectionReason = (RejectionReason)reader.ReadS32(2);
      }
   }
}
