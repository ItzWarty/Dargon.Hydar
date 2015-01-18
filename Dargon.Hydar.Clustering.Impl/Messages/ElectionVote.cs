using System;
using Dargon.Hydar.Clustering.Messages.Helpers;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Clustering.Messages {
   public class ElectionVote : IPortableObject {
      private ElectionCandidate candidate;

      public ElectionVote() { }

      public ElectionVote(ElectionCandidate candidate) {
         this.candidate = candidate;
      }

      public ElectionCandidate Candidate { get { return candidate; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteObject(0, candidate);
      }

      public void Deserialize(IPofReader reader) {
         candidate = reader.ReadObject<ElectionCandidate>(0);
      }
   }
}
