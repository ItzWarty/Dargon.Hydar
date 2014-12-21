using System.Threading;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Clustering.Phases {
   public class MemberPhase : PhaseBase {
      private int leaderAbsentTicks = 0;

      public MemberPhase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory) : base(auditEventBus, context, manageableClusterContext, phaseFactory) {}

      public override void Initialize() {
         base.Initialize();
         RegisterHandler<LeaderHeartBeat>(HandleLeaderHeartBeat);
         RegisterHandler<MemberHeartBeat>(HandleMemberHeartBeat);
      }

      public override void Tick() {
         var nextLeaderAbsentTicks = Interlocked.Increment(ref leaderAbsentTicks);
         if (nextLeaderAbsentTicks > configuration.MaximumHeartBeatInterval) {
            clusterContext.Transition(phaseFactory.CreateElectionPhase());
         }
         SendDataNodeHeartBeat();
      }

      private void SendDataNodeHeartBeat() {
         Send(new MemberHeartBeat(clusterContext.GetCurrentEpoch().Id));
      }

      private void HandleLeaderHeartBeat(IRemoteIdentity remoteIdentity, HydarMessageHeader header, LeaderHeartBeat payload) {
         Interlocked.Exchange(ref leaderAbsentTicks, 0);
         clusterContext.HandlePeerHeartBeat(header.SenderGuid);
      }

      private void HandleMemberHeartBeat(IRemoteIdentity remoteIdentity, HydarMessageHeader header, MemberHeartBeat payload) {
         clusterContext.HandlePeerHeartBeat(header.SenderGuid);
      }
   }
}