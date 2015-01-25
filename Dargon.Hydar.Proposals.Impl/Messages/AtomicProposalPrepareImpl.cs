using Dargon.PortableObjects;
using System;

namespace Dargon.Hydar.Proposals.Messages {
   public class AtomicProposalPrepareImpl<TSubject> : IPortableObject, AtomicProposalPrepare<TSubject> {
      private Guid topicId;
      private Guid proposalId;
      private TSubject entryKey;
      private Proposal<TSubject> proposal;

      public AtomicProposalPrepareImpl() { } 

      public AtomicProposalPrepareImpl(Guid topicId, Guid proposalId, TSubject entryKey, Proposal<TSubject> operation) {
         this.topicId = topicId;
         this.proposalId = proposalId;
         this.entryKey = entryKey;
         this.proposal = proposal;
      }

      public Guid TopicId { get { return topicId; } }
      public Guid ProposalId { get { return proposalId; } }
      public TSubject EntryKey { get { return entryKey; } }
      public Proposal<TSubject> Proposal { get { return proposal; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, topicId);
         writer.WriteGuid(1, proposalId);
         writer.WriteObject(2, entryKey);
         writer.WriteObject(3, proposal);
      }

      public void Deserialize(IPofReader reader) {
         topicId = reader.ReadGuid(0);
         proposalId = reader.ReadGuid(1);
         entryKey = reader.ReadObject<TSubject>(2);
         proposal = reader.ReadObject<Proposal<TSubject>>(3);
      }
   }
}
