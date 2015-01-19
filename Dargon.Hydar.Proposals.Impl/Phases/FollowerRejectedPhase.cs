using System;

namespace Dargon.Hydar.Proposals.Phases {
   public class FollowerRejectedPhase<K, V> : ProposalPhaseBase<K, V> {
      private readonly SubjectState<> subjectState;

      public FollowerRejectedPhase(SubjectState<> subjectState) {
         this.subjectState = subjectState;
      }

      public override void Initialize() {
         base.Initialize();
      }

      public override bool TryBullyWith(SubjectState<> candidate) {
         throw new InvalidOperationException("Nonsensical to bully a rejected context");
      }
   }
}
