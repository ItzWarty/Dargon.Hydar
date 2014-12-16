using Dargon.Audits;

namespace Dargon.Hydar.Grid.ClusterPhases {
   public class InitializationClusterPhase : ClusterPhaseBase {
      public InitializationClusterPhase(AuditEventBus auditEventBus, NodePhaseFactory phaseFactory, HydarContext context) : base(auditEventBus, phaseFactory, context) {}

      public override void Enter() {
         context.Network.Join(context.Node);
         context.SetClusterPhase(phaseFactory.CreateIndeterminatePhase());
      }

      public override void Tick() {
         // do nothing
      }
   }
}
