using Dargon.Audits;

namespace Dargon.Hydar.Grid.Phases {
   public class InitializationPhase : PhaseBase {
      public InitializationPhase(AuditEventBus auditEventBus, NodePhaseFactory phaseFactory, HydarContext context) : base(auditEventBus, phaseFactory, context) {}

      public override void Enter() {
         context.Network.Join(context.Node);
         context.SetPhase(phaseFactory.CreateIndeterminatePhase());
      }

      public override void Tick() {
         // do nothing
      }
   }
}
