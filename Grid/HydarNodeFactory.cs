using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Audits;
using Dargon.Hydar.Grid.ClusterPhases;
using Dargon.Hydar.Networking;

namespace Dargon.Hydar.Grid {
   public class HydarNodeFactory {
      private readonly GridConfiguration configuration;
      private readonly Network network;
      private readonly AuditEventBus auditEventBus;

      public HydarNodeFactory(GridConfiguration configuration, Network network, AuditEventBus auditEventBus) {
         this.configuration = configuration;
         this.network = network;
         this.auditEventBus = auditEventBus;
      }

      public HydarNode Create() {
         var nodePhaseFactory = new NodePhaseFactoryImpl(auditEventBus);
         var node = new HydarNodeImpl(auditEventBus, nodePhaseFactory);
         var context = new HydarContextImpl(auditEventBus, configuration, network, nodePhaseFactory, node);
         nodePhaseFactory.SetContext(context);
         node.SetContext(context);
         context.Initialize();
         return node;
      }
   }
}
