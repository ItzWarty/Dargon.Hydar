using System.Threading;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Clustering.Phases {
   public class MemberPhase : PhaseBase {
      private int leaderAbsentTicks = 0;

      public MemberPhase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory) : base(auditEventBus, context, manageableClusterContext, phaseFactory) {}

      public override void Initialize() {
         base.Initialize();
         RegisterHandler<LeaderHeartBeat>(HandleLeaderHeartBeat);
      }

      public override void Tick() {
         var nextLeaderAbsentTicks = Interlocked.Increment(ref leaderAbsentTicks);
         if (nextLeaderAbsentTicks > configuration.TicksToElection) {
            clusterContext.Transition(phaseFactory.CreateElectionPhase());
         }
         SendDataNodeHeartBeat();
      }

      private void SendDataNodeHeartBeat() {
         Send(new MemberNodeHeartBeat(clusterContext.GetCurrentEpoch().Id));
      }

      private void HandleLeaderHeartBeat(IRemoteIdentity arg1, HydarMessageHeader arg2, LeaderHeartBeat arg3) {
         Interlocked.Exchange(ref leaderAbsentTicks, 0);
      }
   }
}