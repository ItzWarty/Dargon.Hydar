using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Grid.ClusterPhases {
   public class IndeterminateClusterPhase : ClusterPhaseBase {
      private int tickCount = 0;

      // auto-generated ctor
      public IndeterminateClusterPhase(AuditEventBus auditEventBus, NodePhaseFactory phaseFactory, HydarContext context) : base(auditEventBus, phaseFactory, context) {}

      public void Initialize() {
         RegisterHandler<LeaderHeartBeat>(HandleLeaderHeartBeat);
      }

      public override void Tick() {
         tickCount++;
         if (tickCount < context.Configuration.TicksToElection) {
            // do nothing
         } else {
            context.SetClusterPhase(phaseFactory.CreateElectionPhase());
         }
      }

      public void HandleLeaderHeartBeat(IRemoteIdentity remoteIdentity, HydarMessageHeader messageHeader, LeaderHeartBeat heartbeat) {
         // todo: join 
      }
   }
}
