using System;
using System.Collections.Generic;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;

namespace Dargon.Hydar.Grid.ClusterPhases {
   public abstract class ClusterPhaseBase : MessageProcessorBase, IClusterPhase {
      protected ClusterPhaseBase(AuditEventBus auditEventBus, NodePhaseFactory phaseFactory, HydarContext context) 
         : base (auditEventBus, phaseFactory, context) { }

      public virtual void Enter() { }
      public abstract void Tick();
   }
}
