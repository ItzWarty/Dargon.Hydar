using System;

namespace Dargon.Hydar.Proposals {
   public interface SubjectStateManager<TSubject> {
      SubjectState<TSubject> GetOrCreate(TSubject subject);
   }
}
