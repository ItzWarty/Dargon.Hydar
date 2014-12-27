using Dargon.PortableObjects;
using System;

namespace Dargon.Hydar.Clustering.Messages.Helpers {
   public class ElectionCandidate : IPortableObject {
      private Guid id;
      private Guid lastEpochId;

      public ElectionCandidate(Guid id, Guid lastEpochId) {
         this.id = id;
         this.lastEpochId = lastEpochId;
      }

      public Guid Id { get { return id; } }
      public Guid LastEpochId { get { return lastEpochId; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, id);
         writer.WriteGuid(1, lastEpochId);
      }

      public void Deserialize(IPofReader reader) {
         id = reader.ReadGuid(0);
         lastEpochId = reader.ReadGuid(1);
      }
   }
}
