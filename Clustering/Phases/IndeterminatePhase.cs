using System.Threading;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Clustering.Phases {
   public class IndeterminatePhase : PhaseBase {
      private int tickCount = 0;

      // auto-generated ctor
      public IndeterminatePhase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory) : base(auditEventBus, context, manageableClusterContext, phaseFactory) { }

      public override void Initialize() {
         base.Initialize();
         RegisterHandler<LeaderHeartBeat>(HandleLeaderHeartBeat);
         RegisterHandler<MemberHeartBeat>(HandleMemberHeartBeat);
         RegisterHandler<ElectionVote>(HandleElectionVote);
         RegisterNullHandler<ElectionAcknowledgement>();
      }

      public override void Tick() {
         var currentTickCount = Interlocked.Increment(ref tickCount);
         if (currentTickCount < context.Configuration.MaximumHeartBeatInterval) {
            // do nothing
         } else {
            clusterContext.Transition(phaseFactory.CreateElectionPhase());
         }
      }

      public void HandleLeaderHeartBeat(IRemoteIdentity remoteIdentity, HydarMessageHeader messageHeader, LeaderHeartBeat heartbeat) {
         Interlocked.Exchange(ref tickCount, 0);
      }

      private void HandleMemberHeartBeat(IRemoteIdentity remoteIdentity, HydarMessageHeader messageHeader, MemberHeartBeat heartbeat) {
         Interlocked.Exchange(ref tickCount, 0);
      }

      private void HandleElectionVote(IRemoteIdentity arg1, HydarMessageHeader arg2, ElectionVote arg3) {
         clusterContext.Transition(phaseFactory.CreateElectionPhase());
      }
   }
}
