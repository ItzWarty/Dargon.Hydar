namespace Dargon.Hydar.Proposals.Phases {
   public class FollowerAcceptedPhase<K, V> : ProposalPhaseBase<K, V> {
      private SubjectState<> subjectState;
      private ProposalPhaseFactoryImpl<K, V> proposalPhaseFactoryImpl;

      public FollowerAcceptedPhase(SubjectState<> subjectState, ProposalPhaseFactoryImpl<K, V> proposalPhaseFactoryImpl) {
         this.subjectState = subjectState;
         this.proposalPhaseFactoryImpl = proposalPhaseFactoryImpl;
      }

      public override void Step() {
         base.Step();
      }

      public override bool TryBullyWith(SubjectState<> candidate) {
         return false;
      }
   }
}
