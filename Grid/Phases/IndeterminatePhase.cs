using System.Threading;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Grid.Phases {
   public class IndeterminatePhase : PhaseBase {
      private int tickCount = 0;

      // auto-generated ctor
      public IndeterminatePhase(AuditEventBus auditEventBus, NodePhaseFactory phaseFactory, HydarContext context) : base(auditEventBus, phaseFactory, context) {}

      public void Initialize() {
         RegisterHandler<LeaderHeartBeat>(HandleLeaderHeartBeat);
      }

      public override void Tick() {
         tickCount++;
         if (tickCount < context.Configuration.TicksToElection) {
            // do nothing
         } else {
            context.SetPhase(phaseFactory.CreateElectionPhase());
         }
      }

      public void HandleLeaderHeartBeat(IRemoteIdentity remoteIdentity, HydarMessageHeader messageHeader, LeaderHeartBeat heartbeat) {
         // todo: join 
      }
   }
}
