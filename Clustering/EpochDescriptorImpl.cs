using System;
using System.Collections.Generic;
using Dargon.Hydar.Utilities;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering {
   public class EpochDescriptorImpl : EpochDescriptor {
      private readonly Guid id;
      private readonly DateTimeInterval interval;
      private readonly Guid leaderGuid;
      private readonly IReadOnlySet<Guid> participantGuids;
      private readonly SortedList<Guid, ManageablePeerStatus> participantStatusesByGuid;
      private readonly Guid previousId;
      private readonly IReadOnlySet<Guid> previousParticipantGuids;
      private readonly SortedList<Guid, ManageablePeerStatus> previousParticipantStatusesByGuid;

      public EpochDescriptorImpl(Guid id, DateTimeInterval interval, Guid leaderGuid, IReadOnlySet<Guid> participantGuids, SortedList<Guid, ManageablePeerStatus> participantStatusesByGuid, Guid previousId, IReadOnlySet<Guid> previousParticipantGuids, SortedList<Guid, ManageablePeerStatus> previousParticipantStatusesByGuid) {
         this.id = id;
         this.interval = interval;
         this.leaderGuid = leaderGuid;
         this.participantGuids = participantGuids;
         this.participantStatusesByGuid = participantStatusesByGuid;
         this.previousId = previousId;
         this.previousParticipantGuids = previousParticipantGuids;
         this.previousParticipantStatusesByGuid = previousParticipantStatusesByGuid;
      }

      public Guid Id { get { return id; } }
      public DateTimeInterval Interval { get { return interval; } }
      public Guid LeaderGuid { get { return leaderGuid; } }
      public IReadOnlySet<Guid> ParticipantGuids { get { return participantGuids; } }
      public IReadOnlyDictionary<Guid, ManageablePeerStatus> ParticipantStatusesByGuid { get { return (IReadOnlyDictionary<Guid, ManageablePeerStatus>)participantStatusesByGuid; } }
      public Guid PreviousId { get { return previousId; } }
      public IReadOnlySet<Guid> PreviousParticipantGuids { get { return previousParticipantGuids; } }
      public IReadOnlyDictionary<Guid, ManageablePeerStatus> PreviousParticipantStatusesByGuid { get { return (IReadOnlyDictionary<Guid, ManageablePeerStatus>)previousParticipantStatusesByGuid; } }
   }
}