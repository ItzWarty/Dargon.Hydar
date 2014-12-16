using Dargon.Audits;

namespace Dargon.Hydar.Grid.Peering {
   public abstract class PeeringPhaseBase : MessageProcessorBase, IPeeringPhase {
      protected readonly NodePhaseFactory phaseFactory;
      protected readonly PeeringContext peeringContext;

      protected PeeringPhaseBase(AuditEventBus auditEventBus, HydarContext context, NodePhaseFactory phaseFactory) 
         : base (auditEventBus, context) {
         this.phaseFactory = phaseFactory;
         this.peeringContext = context.PeeringContext;
      }

      public virtual void Enter() { }
      public abstract void Tick();
   }
}
