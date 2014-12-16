using System.Threading;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Grid.Peering {
   public class MemberPeeringPhase : PeeringPhaseBase {
      private int leaderAbsentTicks = 0;

      public MemberPeeringPhase(AuditEventBus auditEventBus, HydarContext context, NodePhaseFactory phaseFactory) : base(auditEventBus, context, phaseFactory) {}

      public void Initialize() {
         RegisterHandler<LeaderHeartBeat>(HandleLeaderHeartBeat);
      }

      public override void Tick() {
         var nextLeaderAbsentTicks = Interlocked.Increment(ref leaderAbsentTicks);
         if (nextLeaderAbsentTicks > configuration.TicksToElection) {
            peeringContext.SetPhase(phaseFactory.CreateElectionPhase());
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