using Dargon.Hydar.Proposals.Messages;
using System;

namespace Dargon.Hydar.Proposals {
   public interface AtomicExecutionContext<TSubject> {
      void Execute(TSubject subject, Proposal<TSubject> proposal);
   }
}
