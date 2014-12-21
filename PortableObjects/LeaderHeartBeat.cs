using Dargon.PortableObjects;
using System;
using ItzWarty.Collections;

namespace Dargon.Hydar.PortableObjects {
   public class LeaderHeartBeat : IPortableObject {
      private Guid epochId;
      private ISet<Guid> participantIds; 

      public LeaderHeartBeat() { }

      public LeaderHeartBeat(Guid epochId, ISet<Guid> participantIds) {
         this.epochId = epochId;
         this.participantIds = participantIds;
      }

      public Guid EpochId { get { return epochId; } }
      public IReadOnlySet<Guid> ParticipantIds { get { return participantIds; } } 

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, epochId);
         writer.WriteCollection(1, participantIds);
      }

      public void Deserialize(IPofReader reader) {
         epochId = reader.ReadGuid(0);
         participantIds = reader.ReadCollection<Guid, SortedSet<Guid>>(1);
      }
   }
}
