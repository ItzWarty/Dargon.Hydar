using Dargon.Audits;
using ItzWarty;

namespace Dargon.Hydar.Grid.Phases {
   public class NodePhaseFactoryImpl : NodePhaseFactory {
      private readonly AuditEventBus auditEventBus;
      private HydarContext context;

      public NodePhaseFactoryImpl(AuditEventBus auditEventBus) {
         this.auditEventBus = auditEventBus;
      }

      public void SetContext(HydarContext context) {
         this.context = context;
      }

      public IPhase CreateIndeterminatePhase() {
         return new IndeterminatePhase(auditEventBus, this, context).With(x => x.Initialize());
      }

      public IPhase CreateInitializationPhase() {
         return new InitializationPhase(auditEventBus, this, context);
      }

      public IPhase CreateElectionPhase() {
         return new ElectionPhase(auditEventBus, this, context).With(x => x.Initialize());
      }

      public IPhase CreateLeaderPhase() {
         return new LeaderPhase(auditEventBus, this, context);
      }

      public IPhase CreateMemberPhase() {
         return new MemberPhase(auditEventBus, this, context).With(x => x.Initialize());
      }
   }
}