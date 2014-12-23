using Dargon.PortableObjects;
using System;
using Dargon.Hydar.Utilities;
using ItzWarty.Collections;

namespace Dargon.Hydar.PortableObjects {
   public class LeaderHeartBeat : IPortableObject {
      private Guid epochId;
      private DateTimeInterval interval;
      private ISet<Guid> participantIds; 

      public LeaderHeartBeat() { }

      public LeaderHeartBeat(Guid epochId, DateTimeInterval interval, ISet<Guid> participantIds) {
         this.epochId = epochId;
         this.interval = interval;
         this.participantIds = participantIds;
      }

      public Guid EpochId { get { return epochId; } }
      public DateTimeInterval Interval { get { return interval; } }
      public IReadOnlySet<Guid> ParticipantIds { get { return participantIds; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, epochId);
         writer.WriteObject(1, interval);
         writer.WriteCollection(2, participantIds);
      }

      public void Deserialize(IPofReader reader) {
         epochId = reader.ReadGuid(0);
         interval = reader.ReadObject<DateTimeInterval>(1);
         participantIds = reader.ReadCollection<Guid, SortedSet<Guid>>(2);
      }
   }
}
