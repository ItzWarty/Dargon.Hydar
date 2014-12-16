using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Grid.Peering {
   public class IndeterminatePeeringPhase : PeeringPhaseBase {
      private int tickCount = 0;

      // auto-generated ctor
      public IndeterminatePeeringPhase(AuditEventBus auditEventBus, HydarContext context, NodePhaseFactory phaseFactory) : base(auditEventBus, context, phaseFactory) {}

      public void Initialize() {
         RegisterHandler<LeaderHeartBeat>(HandleLeaderHeartBeat);
      }

      public override void Tick() {
         tickCount++;
         if (tickCount < context.Configuration.TicksToElection) {
            // do nothing
         } else {
            peeringContext.SetPhase(phaseFactory.CreateElectionPhase());
         }
      }

      public void HandleLeaderHeartBeat(IRemoteIdentity remoteIdentity, HydarMessageHeader messageHeader, LeaderHeartBeat heartbeat) {
         // todo: join 
      }
   }
}
