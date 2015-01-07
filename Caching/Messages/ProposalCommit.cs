using Dargon.PortableObjects;
using System;

namespace Dargon.Hydar.Caching.Management {
   public class ProposalCommit : IPortableObject {
      private Guid proposalId;

      public ProposalCommit(Guid proposalId) {
         this.proposalId = proposalId;
      }

      public Guid ProposalId { get { return proposalId; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, proposalId);
      }

      public void Deserialize(IPofReader reader) {
         proposalId = reader.ReadGuid(0);
      }
   }
}
