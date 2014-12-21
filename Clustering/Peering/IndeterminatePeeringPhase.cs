using System.Threading;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Clustering.Peering {
   public class IndeterminatePeeringPhase : PeeringPhaseBase {
      private int tickCount = 0;

      // auto-generated ctor
      public IndeterminatePeeringPhase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory) : base(auditEventBus, context, manageableClusterContext, phaseFactory) { }

      public override void Initialize() {
         base.Initialize();
         RegisterHandler<LeaderHeartBeat>(HandleLeaderHeartBeat);
      }

      public override void Tick() {
         var currentTickCount = Interlocked.Increment(ref tickCount);
         if (currentTickCount < context.Configuration.TicksToElection) {
            // do nothing
         } else {
            clusterContext.Transition(phaseFactory.CreateElectionPhase());
         }
      }

      public void HandleLeaderHeartBeat(IRemoteIdentity remoteIdentity, HydarMessageHeader messageHeader, LeaderHeartBeat heartbeat) {
         Interlocked.Exchange(ref tickCount, 0);
      }
   }
}
