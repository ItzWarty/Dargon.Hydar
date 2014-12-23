using System;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering.Phases {
   public class LeaderPhase : PhaseBase {
      private readonly ISet<Guid> participants;
      private Guid epochId;

      public LeaderPhase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory, ISet<Guid> participants) : base(auditEventBus, context, manageableClusterContext, phaseFactory) {
         this.participants = participants;
      }

      public override void Initialize() {
         base.Initialize();
         RegisterHandler<MemberHeartBeat>(HandleMemberHeartBeat);
         RegisterNullHandler<ElectionAcknowledgement>();
         RegisterNullHandler<ElectionVote>();

         epochId = Guid.NewGuid();
      }

      public override void Enter() {
         base.Enter();

         clusterContext.EnterEpoch(epochId, node.Identifier, participants);
         SendHeartBeat();
      }

      public override void Tick() {
         SendHeartBeat();
      }

      private void SendHeartBeat() {
         Log("Sending heartbeat to {0} participants".F(participants.Count));
         Send(new LeaderHeartBeat(epochId, participants));
      }

      private void HandleMemberHeartBeat(IRemoteIdentity identity, HydarMessageHeader header, MemberHeartBeat payload) {
         clusterContext.HandlePeerHeartBeat(header.SenderGuid);
      }
   }
}