using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Caching.Management {
   public class ProposalReject : IPortableObject {
      private Guid proposalId;

      public ProposalReject(Guid proposalId) {
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
