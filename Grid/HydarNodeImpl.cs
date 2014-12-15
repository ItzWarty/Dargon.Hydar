using System;
using System.Collections.Generic;
using Dargon.Audits;
using Dargon.Hydar.Grid.Phases;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;

namespace Dargon.Hydar.Grid {
   public class HydarNodeImpl : HydarNode {
      private readonly AuditEventBus auditEventBus;
      private readonly NodePhaseFactory phaseFactory;
      private readonly Guid identifier;
      private HydarContext context;

      public HydarNodeImpl(AuditEventBus auditEventBus, NodePhaseFactory phaseFactory) {
         this.auditEventBus = auditEventBus;
         this.phaseFactory = phaseFactory;

         this.identifier = Guid.NewGuid();
      }

      public void SetContext(HydarContext context) {
         this.context = context;
      }

      public Guid Identifier { get { return identifier; } }

      public void Receive(IRemoteIdentity senderIdentity, HydarMessage message) {
         context.Receive(senderIdentity, message);
      }
   }
}