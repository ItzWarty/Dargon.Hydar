using System;
using System.Collections.Generic;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering {
   public class EpochDescriptorImpl : EpochDescriptor {
      private readonly Guid id;
      private readonly Guid leaderGuid;
      private readonly IReadOnlySet<Guid> participantGuids;
      private readonly SortedList<Guid, ManageablePeerStatus> participantStatusesByGuid;

      public EpochDescriptorImpl(Guid id, Guid leaderGuid, IReadOnlySet<Guid> participantGuids, SortedList<Guid, ManageablePeerStatus> participantStatusesByGuid) {
         this.id = id;
         this.leaderGuid = leaderGuid;
         this.participantGuids = participantGuids;
         this.participantStatusesByGuid = participantStatusesByGuid;
      }

      public Guid Id { get { return id; } }
      public Guid LeaderGuid { get { return leaderGuid; } }
      public IReadOnlySet<Guid> ParticipantGuids { get { return participantGuids; } }
      public IReadOnlyDictionary<Guid, ManageablePeerStatus> ParticipantStatusesByGuid { get { return (IReadOnlyDictionary<Guid, ManageablePeerStatus>)participantStatusesByGuid; } }
   }
}