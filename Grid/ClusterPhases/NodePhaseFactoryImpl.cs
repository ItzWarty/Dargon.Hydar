using Dargon.Audits;
using ItzWarty;

namespace Dargon.Hydar.Grid.ClusterPhases {
   public class NodePhaseFactoryImpl : NodePhaseFactory {
      private readonly AuditEventBus auditEventBus;
      private HydarContext context;

      public NodePhaseFactoryImpl(AuditEventBus auditEventBus) {
         this.auditEventBus = auditEventBus;
      }

      public void SetContext(HydarContext context) {
         this.context = context;
      }

      public IClusterPhase CreateIndeterminatePhase() {
         return new IndeterminateClusterPhase(auditEventBus, this, context).With(x => x.Initialize());
      }

      public IClusterPhase CreateInitializationPhase() {
         return new InitializationClusterPhase(auditEventBus, this, context);
      }

      public IClusterPhase CreateElectionPhase() {
         return new ElectionClusterPhase(auditEventBus, this, context).With(x => x.Initialize());
      }

      public IClusterPhase CreateLeaderPhase() {
         return new LeaderClusterPhase(auditEventBus, this, context);
      }

      public IClusterPhase CreateMemberPhase() {
         return new MemberClusterPhase(auditEventBus, this, context).With(x => x.Initialize());
      }
   }
}