using Dargon.PortableObjects;
using System;
using Dargon.Hydar.Utilities;
using ItzWarty.Collections;

namespace Dargon.Hydar.PortableObjects {
   public class LeaderHeartBeat : IPortableObject {
      private Guid epochId;
      private Guid lastEpochId;
      private DateTimeInterval interval;
      private ISet<Guid> participantIds; 

      public LeaderHeartBeat() { }

      public LeaderHeartBeat(Guid epochId, Guid lastEpochId, DateTimeInterval interval, ISet<Guid> participantIds) {
         this.epochId = epochId;
         this.lastEpochId = lastEpochId;
         this.interval = interval;
         this.participantIds = participantIds;
      }

      public Guid EpochId { get { return epochId; } }
      public Guid LastEpochId { get { return lastEpochId; } }
      public DateTimeInterval Interval { get { return interval; } }
      public IReadOnlySet<Guid> ParticipantIds { get { return participantIds; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, epochId);
         writer.WriteGuid(1, lastEpochId);
         writer.WriteObject(2, interval);
         writer.WriteCollection(3, participantIds);
      }

      public void Deserialize(IPofReader reader) {
         epochId = reader.ReadGuid(0);
         lastEpochId = reader.ReadGuid(1);
         interval = reader.ReadObject<DateTimeInterval>(2);
         participantIds = reader.ReadCollection<Guid, SortedSet<Guid>>(3);
      }
   }
}
