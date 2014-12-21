using Dargon.Audits;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Clustering.Phases {
   public abstract class PhaseBase : MessageProcessorBase, IPhase {
      protected readonly NodePhaseFactory phaseFactory;
      protected readonly ManageableClusterContext clusterContext;

      protected PhaseBase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory) 
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
