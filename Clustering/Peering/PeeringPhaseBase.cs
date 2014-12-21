using Dargon.Audits;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Clustering.Peering {
   public abstract class PeeringPhaseBase : MessageProcessorBase, IPeeringPhase {
      protected readonly NodePhaseFactory phaseFactory;
      protected readonly ManageableClusterContext clusterContext;

      protected PeeringPhaseBase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory) 
         : base (auditEventBus, context) {
         this.phaseFactory = phaseFactory;
         this.clusterContext = manageableClusterContext;
      }

      public virtual void Initialize() {

      }

      public virtual void Enter() { }
      public abstract void Tick();
   }
}
