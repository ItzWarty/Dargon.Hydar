using Dargon.Audits;
using ItzWarty;

namespace Dargon.Hydar.Grid.Peering {
   public class NodePhaseFactoryImpl : NodePhaseFactory {
      private readonly AuditEventBus auditEventBus;
      private HydarContext context;

      public NodePhaseFactoryImpl(AuditEventBus auditEventBus) {
         this.auditEventBus = auditEventBus;
      }

      public void SetContext(HydarContext context) {
         this.context = context;
      }

      public IPeeringPhase CreateIndeterminatePhase() {
         return new IndeterminatePeeringPhase(auditEventBus, context, this).With(x => x.Initialize());
      }

      public IPeeringPhase CreateInitializationPhase() {
         return new InitializationPeeringPhase(auditEventBus, context, this);
      }

      public IPeeringPhase CreateElectionPhase() {
         return new ElectionPeeringPhase(auditEventBus, context, this).With(x => x.Initialize());
      }

      public IPeeringPhase CreateLeaderPhase() {
         return new LeaderPeeringPhase(auditEventBus, context, this);
      }

      public IPeeringPhase CreateMemberPhase() {
         return new MemberPeeringPhase(auditEventBus, context, this).With(x => x.Initialize());
      }
   }
}