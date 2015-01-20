using System;
using ItzWarty.Collections;

namespace Dargon.Hydar.Proposals.Messages {
   public interface Proposal<TSubject> {
      TSubject Subject { get; }
      IReadOnlySet<Guid> Participants { get; }
   }
}
