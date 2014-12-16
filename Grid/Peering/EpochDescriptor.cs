using System;
using ItzWarty.Collections;

namespace Dargon.Hydar.Grid.Peering {
   public interface EpochDescriptor {
      Guid Id { get; }
      IReadOnlySet<Guid> ParticipantGuids { get; } 
   }

   public class EpochDescriptorImpl : EpochDescriptor {
      private readonly Guid id;
      private readonly IReadOnlySet<Guid> participantGuids;

      public EpochDescriptorImpl(Guid id, IReadOnlySet<Guid> participantGuids) {
         this.id = id;
         this.participantGuids = participantGuids;
      }

      public Guid Id { get { return id; } }
      public IReadOnlySet<Guid> ParticipantGuids { get { return participantGuids; } }
   }
}