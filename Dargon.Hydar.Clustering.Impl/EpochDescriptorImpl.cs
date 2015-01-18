using System;
using Dargon.Hydar.Clustering.Messages.Helpers;
using Dargon.Hydar.Utilities;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering {
   public class EpochDescriptorImpl : EpochDescriptor {
      public static readonly EpochDescriptorImpl kNullEpochDescriptor = new EpochDescriptorImpl(Guid.Empty, Guid.Empty, new DateTimeInterval(), new HashSet<Guid>(), null);

      private readonly Guid id;
      private readonly Guid leaderId;
      private readonly DateTimeInterval interval;
      private readonly IReadOnlySet<Guid> participantIds;
      private readonly EpochDescriptor previous;

      public EpochDescriptorImpl(Guid id, Guid leaderId, DateTimeInterval interval, IReadOnlySet<Guid> participantIds, EpochDescriptor previous) {
         this.id = id;
         this.leaderId = leaderId;
         this.interval = interval;
         this.participantIds = participantIds;
         this.previous = previous;
      }

      public Guid Id { get { return id; } }
      public Guid LeaderId { get { return leaderId; } }
      public DateTimeInterval Interval { get { return interval; } }
      public IReadOnlySet<Guid> ParticipantIds { get { return participantIds; } }
      public EpochDescriptor Previous { get { return previous; } }

      public EpochSummary ToEpochSummary() {
         return new EpochSummaryImpl(id, leaderId, participantIds, interval);
      }
   }
}