using System;
using Dargon.Audits;
using Dargon.Hydar.PortableObjects;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering.Peering {
   public class LeaderPeeringPhase : PeeringPhaseBase {
      private readonly ISet<Guid> participants;
      private Guid epochId;

      public LeaderPeeringPhase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory, ISet<Guid> participants) : base(auditEventBus, context, manageableClusterContext, phaseFactory) {
         this.participants = participants;
      }

      public override void Initialize() {
         base.Initialize();

         epochId = Guid.NewGuid();
      }

      public override void Enter() {
         base.Enter();

         SendHeartBeat();
      }

      public override void Tick() {
         SendHeartBeat();
      }

      private void SendHeartBeat() {
         foreach (var participant in participants) {
            Console.WriteLine(participant);
         }
         Send(new LeaderHeartBeat(epochId, participants));
      }
   }
}