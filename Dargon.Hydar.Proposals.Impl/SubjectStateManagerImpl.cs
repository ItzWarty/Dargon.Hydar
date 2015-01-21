using ItzWarty.Collections;

namespace Dargon.Hydar.Proposals {
   public class SubjectStateManagerImpl<TSubject> : SubjectStateManager<TSubject> {
      private readonly SubjectStateFactory<TSubject> subjectStateFactory; 
      private readonly IConcurrentDictionary<TSubject, SubjectState<TSubject>> activeProposalContextsByEntryKey = new ConcurrentDictionary<TSubject, SubjectState<TSubject>>();

      public SubjectStateManagerImpl(SubjectStateFactory<TSubject> subjectStateFactory) {
         this.subjectStateFactory = subjectStateFactory;
      }

      public SubjectState<TSubject> GetOrCreate(TSubject subject) {
         return activeProposalContextsByEntryKey.GetOrAdd(
            subject,
            subjectStateFactory.Create
         );
      }
   }
}