using Dargon.PortableObjects;
using System;

namespace Dargon.Hydar.Caching.Management {
   public class ProposalPrepare : IPortableObject {
      private Guid proposalId;
      private EntryOperation entryOperation;

      public ProposalPrepare(Guid proposalId, EntryOperation entryOperation) {
         this.proposalId = proposalId;
         this.entryOperation = entryOperation;
      }

      public Guid ProposalId { get { return proposalId; } }
      public EntryOperation EntryOperation { get { return entryOperation; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, proposalId);
         writer.WriteObject(1, entryOperation);
      }

      public void Deserialize(IPofReader reader) {
         proposalId = reader.ReadGuid(0);
         entryOperation = reader.ReadObject<EntryOperation>(1);
      }
   }
}
