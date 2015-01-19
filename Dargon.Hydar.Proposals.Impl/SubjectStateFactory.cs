namespace Dargon.Hydar.Proposals {
   public interface SubjectStateFactory<TSubject> {
      SubjectState<TSubject> Create(TSubject subject);
   }

   public class SubjectStateFactoryImpl<TSubject> : SubjectStateFactory<TSubject> {
      private readonly AtomicExecutionContext<TSubject> atomicExecutionContext;

      public SubjectStateFactoryImpl(AtomicExecutionContext<TSubject> atomicExecutionContext) {
         this.atomicExecutionContext = atomicExecutionContext;
      }

      public SubjectState<TSubject> Create(TSubject subject) {
         return new SubjectStateImpl<TSubject>(atomicExecutionContext, subject);
      }
   }
}