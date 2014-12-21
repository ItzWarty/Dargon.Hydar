using System;
using Dargon.Audits;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering.Phases {
   public class NodePhaseFactoryImpl : NodePhaseFactory {
      private readonly AuditEventBus auditEventBus;
      private HydarContext context;
      private ManageableClusterContext clusterContext;

      public NodePhaseFactoryImpl(AuditEventBus auditEventBus) {
         this.auditEventBus = auditEventBus;
      }

      public void SetContext(HydarContext context) {
         this.context = context;
      }

      public void SetClusterContext(ManageableClusterContext clusterContext) {
         this.clusterContext = clusterContext;
      }

      public IPhase CreateIndeterminatePhase() {
         return new IndeterminatePhase(auditEventBus, context, clusterContext, this).With(x => x.Initialize());
      }

      public IPhase CreateInitializationPhase() {
         return new InitializationPhase(auditEventBus, context, clusterContext, this);
      }

      public IPhase CreateElectionPhase() {
         return new ElectionPhase(auditEventBus, context, clusterContext, this).With(x => x.Initialize());
      }

      public IPhase CreateLeaderPhase(ISet<Guid> participants) {
         return new LeaderPhase(auditEventBus, context, clusterContext, this, participants).With(x => x.Initialize());
      }

      public IPhase CreateMemberPhase() {
         return new MemberPhase(auditEventBus, context, clusterContext, this).With(x => x.Initialize());
      }
   }
}