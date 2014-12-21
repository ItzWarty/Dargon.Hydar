using System;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering {
   public class EpochDescriptorImpl : EpochDescriptor {
      private readonly Guid id;
      private readonly Guid leaderGuid;
      private readonly IReadOnlySet<Guid> participantGuids;

      public EpochDescriptorImpl(Guid id, Guid leaderGuid, IReadOnlySet<Guid> participantGuids) {
         this.id = id;
         this.leaderGuid = leaderGuid;
         this.participantGuids = participantGuids;
      }

      public Guid Id { get { return id; } }
      public Guid LeaderGuid { get { return leaderGuid; } }
      public IReadOnlySet<Guid> ParticipantGuids { get { return participantGuids; } }
   }
}