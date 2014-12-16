using Dargon.Audits;

namespace Dargon.Hydar.Grid.Peering {
   public class InitializationPeeringPhase : PeeringPhaseBase {
      public InitializationPeeringPhase(AuditEventBus auditEventBus, HydarContext context, NodePhaseFactory phaseFactory) : base(auditEventBus, context, phaseFactory) {}

      public override void Enter() {
         context.Network.Join(context.Node);
         peeringContext.SetPhase(phaseFactory.CreateIndeterminatePhase());
      }

      public override void Tick() {
         // do nothing
      }
   }
}
