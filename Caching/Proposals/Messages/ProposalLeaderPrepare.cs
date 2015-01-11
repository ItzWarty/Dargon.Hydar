using System;
using Dargon.Hydar.Caching.Data.Operations;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Caching.Proposals.Messages {
   public class ProposalLeaderPrepare<K> : IPortableObject, IProposalMessage {
      private Guid proposalId;
      private K entryKey;
      private EntryOperation operation;

      public ProposalLeaderPrepare(Guid proposalId, K entryKey, EntryOperation operation) {
         this.proposalId = proposalId;
         this.entryKey = entryKey;
         this.operation = operation;
      }

      public Guid ProposalId { get { return proposalId; } }
      public K EntryKey { get { return entryKey; } }
      public EntryOperation Operation { get { return operation; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, proposalId);
         writer.WriteObject(1, entryKey);
         writer.WriteObject(2, operation);
      }

      public void Deserialize(IPofReader reader) {
         proposalId = reader.ReadGuid(0);
         entryKey = reader.ReadObject<K>(1);
         operation = reader.ReadObject<EntryOperation>(2);
      }
   }
}
