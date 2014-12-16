using Dargon.Audits;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Grid.ClusterPhases {
   public class LeaderClusterPhase : ClusterPhaseBase {
      public LeaderClusterPhase(AuditEventBus auditEventBus, NodePhaseFactory phaseFactory, HydarContext context) : base(auditEventBus, phaseFactory, context) {}

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