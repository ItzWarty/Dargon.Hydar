using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;
using ItzWarty.Collections;
using System;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Clustering.Phases {
   public class LeaderPhase : PhaseBase {
      private readonly ISet<Guid> participants;

      public LeaderPhase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory, ISet<Guid> participants) : base(auditEventBus, context, manageableClusterContext, phaseFactory) {
         this.participants = participants;
      }

      public override void Initialize() {
         base.Initialize();
         RegisterHandler<MemberHeartBeat>(HandleMemberHeartBeat);
         RegisterNullHandler<ElectionAcknowledgement>();
         RegisterNullHandler<ElectionVote>();
      }

      public override void Enter() {
         base.Enter();

         StartNewEpoch();
         SendHeartBeat();
      }

      private void StartNewEpoch() {
         var lastEpochId = clusterContext.GetCurrentEpoch().Id;
         var epochId = Guid.NewGuid();
         var epochStartTime = DateTime.Now;
         var epochExpirationTime = epochStartTime + TimeSpan.FromMilliseconds(configuration.EpochDurationMilliseconds);
         var epochTimeInterval = new DateTimeInterval(epochStartTime, epochExpirationTime);
         clusterContext.EnterEpoch(epochId, epochTimeInterval, node.Identifier, participants, lastEpochId);
      }

      public override void Tick() {
         var epoch = clusterContext.GetCurrentEpoch();
         if (DateTime.Now >= epoch.Interval.End) {
            clusterContext.Transition(phaseFactory.CreateElectionPhase(epoch.Id));
         }
         SendHeartBeat();
      }

      private void SendHeartBeat() {
         Log("Sending heartbeat to {0} participants".F(participants.Count));
         var epoch = clusterContext.GetCurrentEpoch();
         Send(new LeaderHeartBeat(epoch.Id, epoch.PreviousId, epoch.Interval, participants));
      }

      private void HandleMemberHeartBeat(IRemoteIdentity identity, HydarMessageHeader header, MemberHeartBeat payload) {
         clusterContext.HandlePeerHeartBeat(header.SenderGuid);
      }
   }
}