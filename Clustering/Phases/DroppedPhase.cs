using System;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Clustering.Phases {
   public class DroppedPhase : PhaseBase {
      private readonly DateTime epochEndTime;

      public DroppedPhase(
         AuditEventBus auditEventBus, 
         HydarContext context, 
         ManageableClusterContext manageableClusterContext, 
         NodePhaseFactory phaseFactory, 
         DateTime epochEndTime
      ) : base(auditEventBus, context, manageableClusterContext, phaseFactory) {
         this.epochEndTime = epochEndTime;
      }

      public override void Initialize() {
         base.Initialize();

         RegisterNullHandler<LeaderHeartBeat>();
         RegisterNullHandler<MemberHeartBeat>();
         RegisterNullHandler<ElectionAcknowledgement>();
         RegisterHandler<ElectionVote>(HandleElectionVote);
      }

      public override void Tick() { }

      private void HandleElectionVote(IRemoteIdentity arg1, HydarMessageHeader arg2, ElectionVote arg3) {
         if (DateTime.Now >= epochEndTime) {
            clusterContext.Transition(phaseFactory.CreateElectionPhase());
         }
      }
   }
}
