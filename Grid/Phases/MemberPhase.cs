using System.Threading;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Grid.Phases {
   public class MemberPhase : PhaseBase {
      private int leaderAbsentTicks = 0;

      public MemberPhase(AuditEventBus auditEventBus, NodePhaseFactory phaseFactory, HydarContext context) : base(auditEventBus, phaseFactory, context) {}

      public void Initialize() {
         RegisterHandler<LeaderHeartBeat>(HandleLeaderHeartBeat);
      }

      public override void Tick() {
         var nextLeaderAbsentTicks = Interlocked.Increment(ref leaderAbsentTicks);
         if (nextLeaderAbsentTicks > configuration.TicksToElection) {
            context.SetPhase(phaseFactory.CreateElectionPhase());
         }
         SendDataNodeHeartBeat();
      }

      private void SendDataNodeHeartBeat() {
         Send(new DataNodeHeartBeat());
      }

      private void HandleLeaderHeartBeat(IRemoteIdentity arg1, HydarMessageHeader arg2, LeaderHeartBeat arg3) {
         Interlocked.Exchange(ref leaderAbsentTicks, 0);
      }
   }
}