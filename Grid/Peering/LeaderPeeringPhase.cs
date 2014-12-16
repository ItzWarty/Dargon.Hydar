using Dargon.Audits;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Grid.Peering {
   public class LeaderPeeringPhase : PeeringPhaseBase {
      public LeaderPeeringPhase(AuditEventBus auditEventBus, HydarContext context, NodePhaseFactory phaseFactory) : base(auditEventBus, context, phaseFactory) {}

      public override void Enter() {
         base.Enter();

         SendHeartBeat();
      }

      public override void Tick() {
         SendHeartBeat();
      }

      private void SendHeartBeat() {
         Send(new LeaderHeartBeat());
      }
   }
}