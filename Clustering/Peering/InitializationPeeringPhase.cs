using Dargon.Audits;

namespace Dargon.Hydar.Clustering.Peering {
   public class InitializationPeeringPhase : PeeringPhaseBase {
      public InitializationPeeringPhase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory) : base(auditEventBus, context, manageableClusterContext, phaseFactory) { }

      public override void Enter() {
         context.Network.Join(context.Node);
         clusterContext.Transition(phaseFactory.CreateIndeterminatePhase());
      }

      public override void Tick() {
         // do nothing
      }
   }
}
