using System;
using System.Collections.Generic;
using Dargon.Hydar.Clustering.Discovery;
using Dargon.Hydar.Clustering.Messages.Helpers;
using Dargon.Hydar.Utilities;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering {
   public class EpochDescriptorImpl : EpochDescriptor {
      private readonly Guid id;
      private readonly Guid leaderId;
      private readonly DateTimeInterval interval;
      private readonly IReadOnlySet<Guid> participantIds;
      private readonly SortedList<Guid, ManageablePeerStatus> participantStatusesByGuid;
      private readonly EpochDescriptor previous;

      public EpochDescriptorImpl(Guid id, Guid leaderId, DateTimeInterval interval, IReadOnlySet<Guid> participantIds, SortedList<Guid, ManageablePeerStatus> participantStatusesByGuid, EpochDescriptor previous) {
         this.id = id;
         this.leaderId = leaderId;
         this.interval = interval;
         this.participantIds = participantIds;
         this.participantStatusesByGuid = participantStatusesByGuid;
         this.previous = previous;
      }

      public Guid Id { get { return id; } }
      public Guid LeaderId { get { return leaderId; } }
      public DateTimeInterval Interval { get { return interval; } }
      public IReadOnlySet<Guid> ParticipantIds { get { return participantIds; } }
      public IReadOnlyDictionary<Guid, ManageablePeerStatus> ParticipantStatusesByGuid { get { return (IReadOnlyDictionary<Guid, ManageablePeerStatus>)participantStatusesByGuid; } }
      public EpochDescriptor Previous { get { return previous; } }

      public EpochSummary ToEpochSummary() {
         return new EpochSummary(id, leaderId, participantIds);
      }
   }
}