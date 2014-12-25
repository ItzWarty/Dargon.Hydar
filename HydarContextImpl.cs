using System;
using System.Threading;
using Dargon.Audits;
using Dargon.Hydar.Clustering;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar {
   public class HydarContextImpl : HydarContext {
      private readonly AuditEventBus auditEventBus;
      private readonly ClusteringConfiguration configuration;
      private readonly Network network;
      private readonly NetworkNode node;
      private readonly ConcurrentSet<HydarSubsystem> subsystems = new ConcurrentSet<HydarSubsystem>();
      private ManageableClusterContext clusteringContext;
      private Timer timer;

      public HydarContextImpl(AuditEventBus auditEventBus, ClusteringConfiguration configuration, Network network, NetworkNode node) {
         this.auditEventBus = auditEventBus;
         this.configuration = configuration;
         this.network = network;
         this.node = node;
      }

      public ClusteringConfiguration Configuration { get { return configuration; } }
      public Network Network { get { return network; } }
      public NetworkNode Node { get { return node; } }
      public ClusterContext ClusterContext { get { return clusteringContext; } }

      public void SetClusteringContext(ManageableClusterContext clusteringContext) {
         this.clusteringContext = clusteringContext;
      }

      public void Initialize() {
         timer = new Timer((x) => {
            clusteringContext.Tick();
            foreach (var subsystem in subsystems) {
               subsystem.Tick();
            }
         }, null, configuration.TickIntervalMillis, configuration.TickIntervalMillis);
      }

      public void RegisterSubsystem(HydarSubsystem subsystem) {
         subsystems.TryAdd(subsystem);
      }

      public void Dispatch(IRemoteIdentity senderIdentity, HydarMessage message) {
         bool dispatched = false;
         if (clusteringContext.Process(senderIdentity, message)) {
            dispatched = true;
         } else {
            foreach (var dispatcher in subsystems) {
               if (dispatcher.Dispatch(senderIdentity, message)) {
                  dispatched = true;
                  break;
               }
            }
         }
         if (!dispatched) {
            auditEventBus.Error(HydarConstants.kAuditEventBusErrorKey, "Unknown Payload Type", "Connectivity Phase: {0}, Payload Type: {1}".F(ClusterContext.__DebugCurrentPhase.GetType(), message.Payload.GetType().FullName));
         }
      }

      public void Log(string x) {
         Console.WriteLine(node.Identifier.ToString("n").Substring(0, 8) + " " + x);
      }
   }
}