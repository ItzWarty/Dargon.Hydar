using Dargon.Hydar.Utilities;
using Dargon.PortableObjects;
using ItzWarty.Collections;
using System;

namespace Dargon.Hydar.PortableObjects {
   public class LeaderHeartBeat : IPortableObject {
      private Guid epochId;
      private Guid lastEpochId;
      private DateTimeInterval interval;
      private IReadOnlySet<Guid> participantIds;
      private IReadOnlySet<Guid> lastParticipantIds;

      public LeaderHeartBeat() { }

      public LeaderHeartBeat(Guid epochId, Guid lastEpochId, DateTimeInterval interval, IReadOnlySet<Guid> participantIds, IReadOnlySet<Guid> lastParticipantIds) {
         this.epochId = epochId;
         this.lastEpochId = lastEpochId;
         this.interval = interval;
         this.participantIds = participantIds;
         this.lastParticipantIds = lastParticipantIds;
      }

      public Guid EpochId { get { return epochId; } }
      public Guid LastEpochId { get { return lastEpochId; } }
      public DateTimeInterval Interval { get { return interval; } }
      public IReadOnlySet<Guid> ParticipantIds { get { return participantIds; } }
      public IReadOnlySet<Guid> LastParticipantIds { get { return lastParticipantIds; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, epochId);
         writer.WriteGuid(1, lastEpochId);
         writer.WriteObject(2, interval);
         writer.WriteCollection(3, participantIds);
         writer.WriteCollection(4, lastParticipantIds);
      }

      public void Deserialize(IPofReader reader) {
         epochId = reader.ReadGuid(0);
         lastEpochId = reader.ReadGuid(1);
         interval = reader.ReadObject<DateTimeInterval>(2);
         participantIds = reader.ReadCollection<Guid, SortedSet<Guid>>(3);
         lastParticipantIds = reader.ReadCollection<Guid, SortedSet<Guid>>(4);
      }
   }
}
