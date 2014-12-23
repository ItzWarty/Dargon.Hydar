using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Clustering.Phases {
   public class DroppedPhase : IPhase {
      private readonly AuditEventBus auditEventBus;
      private readonly HydarContext context;
      private readonly ManageableClusterContext manageableClusterContext;
      private readonly NodePhaseFactory phaseFactory;

      public DroppedPhase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory) {
         this.auditEventBus = auditEventBus;
         this.context = context;
         this.manageableClusterContext = manageableClusterContext;
         this.phaseFactory = phaseFactory;
      }

      public void Enter() { }
      public void Tick() { }
      public bool Process(IRemoteIdentity sender, HydarMessage message) {
         if (message.Payload is ElectionVote) {
            manageableClusterContext.Transition(phaseFactory.CreateElectionPhase());
            return true;
         }
         return false;
      }
   }
}
