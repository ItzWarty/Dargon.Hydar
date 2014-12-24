using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Clustering.Phases {
   public class DroppedPhase : PhaseBase {
      public DroppedPhase(
         AuditEventBus auditEventBus, 
         HydarContext context, 
         ManageableClusterContext manageableClusterContext, 
         NodePhaseFactory phaseFactory
      ) : base(auditEventBus, context, manageableClusterContext, phaseFactory) {
      }

      public override void Initialize() {
         base.Initialize();

         RegisterNullHandler<LeaderHeartBeat>();
         RegisterNullHandler<MemberHeartBeat>();
         RegisterHandler<ElectionVote>(HandleElectionVote);
      }

      public override void Tick() { }

      private void HandleElectionVote(IRemoteIdentity arg1, HydarMessageHeader arg2, ElectionVote arg3) {
         clusterContext.Transition(phaseFactory.CreateElectionPhase());
      }
   }
}
