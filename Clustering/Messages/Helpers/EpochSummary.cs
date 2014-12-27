using Dargon.PortableObjects;
using ItzWarty.Collections;
using System;

namespace Dargon.Hydar.Clustering.Messages.Helpers {
   public class EpochSummary : IPortableObject {
      private Guid epochId;
      private Guid leaderId;
      private IReadOnlySet<Guid> participantIds;

      public EpochSummary(Guid epochId, Guid leaderId, IReadOnlySet<Guid> participantIds) {
         this.epochId = epochId;
         this.leaderId = leaderId;
         this.participantIds = participantIds;
      }

      public Guid EpochId { get { return epochId; } }
      public Guid LeaderId { get { return leaderId; } }
      public IReadOnlySet<Guid> ParticipantIds { get { return participantIds; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, epochId);
         writer.WriteGuid(1, leaderId);
         writer.WriteCollection(2, participantIds);
      }

      public void Deserialize(IPofReader reader) {
         epochId = reader.ReadGuid(0);
         leaderId = reader.ReadGuid(1);
         participantIds = reader.ReadCollection<Guid, HashSet<Guid>>(2);
      }
   }
}
