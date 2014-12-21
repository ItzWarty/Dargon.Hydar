using System;
using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public class ElectionVote : IPortableObject {
      private Guid candidateGuid;

      public ElectionVote() { }

      public ElectionVote(Guid candidateGuid) {
         this.candidateGuid = candidateGuid;
      }

      public Guid CandidateGuid { get { return candidateGuid; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, candidateGuid);
      }

      public void Deserialize(IPofReader reader) {
         candidateGuid = reader.ReadGuid(0);
      }
   }
}
