using System;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Proposals.Messages {
   public class AtomicProposalCommitImpl : IPortableObject, AtomicProposalMessage {
      private Guid topicId;
      private Guid proposalId;

      public AtomicProposalCommitImpl() { }

      public AtomicProposalCommitImpl(Guid topicId, Guid proposalId) {
         this.topicId = topicId;
         this.proposalId = proposalId;
      }

      public Guid TopicId {  get { return topicId; } }
      public Guid ProposalId { get { return proposalId; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, topicId);
         writer.WriteGuid(1, proposalId);
      }

      public void Deserialize(IPofReader reader) {
         topicId = reader.ReadGuid(0);
         proposalId = reader.ReadGuid(1);
      }
   }
}
