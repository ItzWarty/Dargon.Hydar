using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Proposals {
   public interface SubjectStateManager<TSubject> {
      SubjectState<TSubject> GetOrCreate(TSubject subject);
   }

   public class SubjectStateManagerImpl<TSubject> : SubjectStateManager<TSubject> {
      public SubjectState<TSubject> GetOrCreate(TSubject subject) {
         throw new NotImplementedException();
      }
   }
}
