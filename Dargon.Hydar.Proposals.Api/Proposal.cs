using System;
using ItzWarty.Collections;

namespace Dargon.Hydar.Proposals {
   public interface Proposal<TSubject> {
      TSubject Subject { get; }
      IReadOnlySet<Guid> Participants { get; }
   }
}
