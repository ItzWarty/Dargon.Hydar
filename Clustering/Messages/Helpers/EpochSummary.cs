using Dargon.Hydar.Utilities;
using Dargon.PortableObjects;
using ItzWarty.Collections;
using System;

namespace Dargon.Hydar.Clustering.Messages.Helpers {
   public class EpochSummary : IPortableObject {
      public static readonly EpochSummary kNullEpochSummary = new EpochSummary(Guid.Empty, Guid.Empty, new HashSet<Guid>(), new DateTimeInterval());

      private Guid epochId;
      private Guid leaderId;
      private IReadOnlySet<Guid> participantIds;
      private DateTimeInterval interval;

      public EpochSummary() { }

      public EpochSummary(Guid epochId, Guid leaderId, IReadOnlySet<Guid> participantIds, DateTimeInterval interval) {
         this.epochId = epochId;
         this.leaderId = leaderId;
         this.participantIds = participantIds;
         this.interval = interval;
      }

      public Guid EpochId { get { return epochId; } }
      public Guid LeaderId { get { return leaderId; } }
      public IReadOnlySet<Guid> ParticipantIds { get { return participantIds; } }
      public DateTimeInterval Interval { get { return interval; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, epochId);
         writer.WriteGuid(1, leaderId);
         writer.WriteCollection(2, participantIds);
         writer.WriteObject(3, interval);
      }

      public void Deserialize(IPofReader reader) {
         epochId = reader.ReadGuid(0);
         leaderId = reader.ReadGuid(1);
         participantIds = reader.ReadCollection<Guid, HashSet<Guid>>(2);
         interval = reader.ReadObject<DateTimeInterval>(3);
      }
   }
}
