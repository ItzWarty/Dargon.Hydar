using System;
using Dargon.Audits;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering.Peering {
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

      public IPeeringPhase CreateIndeterminatePhase() {
         return new IndeterminatePeeringPhase(auditEventBus, context, clusterContext, this).With(x => x.Initialize());
      }

      public IPeeringPhase CreateInitializationPhase() {
         return new InitializationPeeringPhase(auditEventBus, context, clusterContext, this);
      }

      public IPeeringPhase CreateElectionPhase() {
         return new ElectionPeeringPhase(auditEventBus, context, clusterContext, this).With(x => x.Initialize());
      }

      public IPeeringPhase CreateLeaderPhase(ISet<Guid> participants) {
         return new LeaderPeeringPhase(auditEventBus, context, clusterContext, this, participants);
      }

      public IPeeringPhase CreateMemberPhase() {
         return new MemberPeeringPhase(auditEventBus, context, clusterContext, this).With(x => x.Initialize());
      }
   }
}