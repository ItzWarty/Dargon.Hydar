using System;
using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public class ElectionVote : IPortableObject {
      private Guid candidateGuid;
      private Guid lastEpochId;

      public ElectionVote() { }

      public ElectionVote(Guid candidateGuid, Guid lastEpochId) {
         this.candidateGuid = candidateGuid;
         this.lastEpochId = lastEpochId;
      }

      public Guid CandidateGuid { get { return candidateGuid; } }
      public Guid LastEpochId { get { return lastEpochId; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, candidateGuid);
         writer.WriteGuid(1, lastEpochId);
      }

      public void Deserialize(IPofReader reader) {
         candidateGuid = reader.ReadGuid(0);
         lastEpochId = reader.ReadGuid(1);
      }
   }
}
