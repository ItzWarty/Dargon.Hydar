using System;
using Dargon.Audits;
using Dargon.Hydar.Clustering;
using Dargon.Hydar.Clustering.Phases;
using Dargon.Hydar.Networking;

namespace Dargon.Hydar {
   public class HydarFactory {
      private readonly ClusteringConfiguration configuration;
      private readonly Network network;
      private readonly AuditEventBus auditEventBus;

      public HydarFactory(ClusteringConfiguration configuration, Network network, AuditEventBus auditEventBus) {
         this.configuration = configuration;
         this.network = network;
         this.auditEventBus = auditEventBus;
      }

      public NetworkNode Create() {
         var identifier = Guid.NewGuid();
         var nodePhaseFactory = new NodePhaseFactoryImpl(auditEventBus);
         var node = new NetworkNodeImpl(identifier);
         var context = new HydarContextImpl(auditEventBus, configuration, network, node);
         var clusterContext = new ClusterContextImpl(context, configuration, nodePhaseFactory);
         context.SetClusteringContext(clusterContext);
         nodePhaseFactory.SetContext(context);
         nodePhaseFactory.SetClusterContext(clusterContext);
         node.SetContext(context);
         clusterContext.Initialize();
         context.Initialize();
         return node;
      }
   }
}
