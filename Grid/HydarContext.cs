using System;
using System.Threading;
using Dargon.Audits;
using Dargon.Hydar.Grid.ClusterPhases;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;
using ItzWarty.Threading;

namespace Dargon.Hydar.Grid {
   public interface HydarContext {
      GridConfiguration Configuration { get; }
      Network Network { get; }
      HydarNode Node { get; }
      void SetClusterPhase(IClusterPhase clusterPhase);
      void Receive(IRemoteIdentity senderIdentity, HydarMessage message);
      void Log(string x);
   }

   public class HydarContextImpl : HydarContext {
      private readonly AuditEventBus auditEventBus;
      private readonly GridConfiguration configuration;
      private readonly Network network;
      private readonly NodePhaseFactory phaseFactory;
      private readonly object synchronization = new object();
      private readonly HydarNode node;
      private IClusterPhase currentClusterPhase;
      private Timer timer;

      public HydarContextImpl(AuditEventBus auditEventBus, GridConfiguration configuration, Network network, NodePhaseFactory phaseFactory, HydarNode node) {
         this.auditEventBus = auditEventBus;
         this.configuration = configuration;
         this.network = network;
         this.phaseFactory = phaseFactory;
         this.node = node;
         this.currentClusterPhase = null;
      }

      public GridConfiguration Configuration { get { return configuration; } }
      public Network Network { get { return network; } }
      public NodePhaseFactory PhaseFactory { get { return phaseFactory; } }
      public HydarNode Node { get { return node; } }

      public void Initialize() {
         currentClusterPhase = phaseFactory.CreateInitializationPhase();
         SetClusterPhase(currentClusterPhase);

         timer = new Timer((e) => {
            lock (synchronization) {
               currentClusterPhase.Tick();
            }
         }, null, configuration.TickIntervalMillis, configuration.TickIntervalMillis);
      }

      public void SetClusterPhase(IClusterPhase clusterPhase) {
         lock (synchronization) {
            clusterPhase.ThrowIfNull("phase");
            Log("=> " + clusterPhase);
            currentClusterPhase = clusterPhase;
            currentClusterPhase.Enter();
         }
      }

      public void Receive(IRemoteIdentity senderIdentity, HydarMessage message) {
         if (currentClusterPhase.Process(senderIdentity, message)) {
            // do nothing
         } else {
            auditEventBus.Error(GlobalHydarConfiguration.kAuditEventBusErrorKey, "Unknown Payload Type", "Connectivity Phase: {0}, Payload Type: {1}".F(currentClusterPhase.GetType(), message.Payload.GetType().FullName));
         }
      }

      public void Log(string x) {
         Console.WriteLine(node.Identifier.ToString("n").Substring(0, 8) + " " + x);
      }
   }
}
