using System;
using System.Threading;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;

namespace Dargon.Hydar.Clustering.Phases {
   public class MemberPhase : PhaseBase {
      private int leaderAbsentTicks = 0;

      public MemberPhase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory) : base(auditEventBus, context, manageableClusterContext, phaseFactory) {}

      public override void Initialize() {
         base.Initialize();
         RegisterHandler<LeaderHeartBeat>(HandleLeaderHeartBeat);
         RegisterHandler<MemberHeartBeat>(HandleMemberHeartBeat);
         RegisterNullHandler<ElectionAcknowledgement>();
         RegisterNullHandler<ElectionVote>();
      }

      public override void Tick() {
         var currentEpoch = clusterContext.GetCurrentEpoch();
         var nextLeaderAbsentTicks = Interlocked.Increment(ref leaderAbsentTicks);
         if (nextLeaderAbsentTicks > configuration.MaximumHeartBeatInterval) {
            Log("Leader missed {0} heartbeats".F(configuration.MaximumHeartBeatInterval));
            clusterContext.Transition(phaseFactory.CreateElectionPhase(currentEpoch.Id));
         } else if (DateTime.Now >= currentEpoch.Interval.End) {
            Log("End of epoch {0}".F(currentEpoch.Id.ToString("n").Substring(0, 8)));
            clusterContext.Transition(phaseFactory.CreateElectionPhase(currentEpoch.Id));
         }
         SendDataNodeHeartBeat();
      }

      private void SendDataNodeHeartBeat() {
         Send(new MemberHeartBeat(clusterContext.GetCurrentEpoch().Id));
      }

      private void HandleLeaderHeartBeat(IRemoteIdentity remoteIdentity, HydarMessageHeader header, LeaderHeartBeat payload) {
         Interlocked.Exchange(ref leaderAbsentTicks, 0);
         clusterContext.HandlePeerHeartBeat(header.SenderGuid);
         if (payload.EpochId != clusterContext.GetCurrentEpoch().Id) {
            clusterContext.EnterEpoch(payload.EpochId, payload.Interval, header.SenderGuid, payload.ParticipantIds, payload.LastEpochId, payload.LastParticipantIds);
         }
      }

      private void HandleMemberHeartBeat(IRemoteIdentity remoteIdentity, HydarMessageHeader header, MemberHeartBeat payload) {
         clusterContext.HandlePeerHeartBeat(header.SenderGuid);
      }
   }
}