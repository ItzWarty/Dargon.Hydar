using System;
using Dargon.Hydar.Caching.Data.Operations;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Proposals.Messages {
   public class AtomicProposalPrepareImpl<K> : IPortableObject, AtomicProposalPrepare {
      private Guid topicId;
      private Guid proposalId;
      private K entryKey;
      private EntryOperation operation;

      public AtomicProposalPrepareImpl(Guid topicId, Guid proposalId, K entryKey, EntryOperation operation) {
         this.topicId = topicId;
         this.proposalId = proposalId;
         this.entryKey = entryKey;
         this.operation = operation;
      }

      public Guid TopicId { get { return topicId; } }
      public Guid ProposalId { get { return proposalId; } }
      public K EntryKey { get { return entryKey; } }
      public EntryOperation Operation { get { return operation; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, topicId);
         writer.WriteGuid(1, proposalId);
         writer.WriteObject(2, entryKey);
         writer.WriteObject(3, operation);
      }

      public void Deserialize(IPofReader reader) {
         topicId = reader.ReadGuid(0);
         proposalId = reader.ReadGuid(1);
         entryKey = reader.ReadObject<K>(2);
         operation = reader.ReadObject<EntryOperation>(3);
      }
   }
}
