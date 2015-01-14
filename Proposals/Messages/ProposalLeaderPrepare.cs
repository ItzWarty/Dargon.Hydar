using System;
using Dargon.Hydar.Caching.Data.Operations;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Proposals.Messages {
   public class ProposalLeaderPrepare<K> : IPortableObject, IProposalMessage {
      private Guid cacheId;
      private Guid proposalId;
      private K entryKey;
      private EntryOperation operation;

      public ProposalLeaderPrepare(Guid cacheId, Guid proposalId, K entryKey, EntryOperation operation) {
         this.cacheId = cacheId;
         this.proposalId = proposalId;
         this.entryKey = entryKey;
         this.operation = operation;
      }

      public Guid CacheId { get { return cacheId; } }
      public Guid ProposalId { get { return proposalId; } }
      public K EntryKey { get { return entryKey; } }
      public EntryOperation Operation { get { return operation; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, cacheId);
         writer.WriteGuid(1, proposalId);
         writer.WriteObject(2, entryKey);
         writer.WriteObject(3, operation);
      }

      public void Deserialize(IPofReader reader) {
         cacheId = reader.ReadGuid(0);
         proposalId = reader.ReadGuid(1);
         entryKey = reader.ReadObject<K>(2);
         operation = reader.ReadObject<EntryOperation>(3);
      }
   }
}
