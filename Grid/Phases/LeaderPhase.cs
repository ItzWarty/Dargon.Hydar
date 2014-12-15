using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Grid.Phases {
   public class LeaderPhase : PhaseBase {
      public LeaderPhase(AuditEventBus auditEventBus, NodePhaseFactory phaseFactory, HydarContext context) : base(auditEventBus, phaseFactory, context) {}

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