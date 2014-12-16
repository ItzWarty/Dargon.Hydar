using System;
using System.Threading;
using Dargon.Audits;
using Dargon.Hydar.Grid.Phases;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;
using ItzWarty.Threading;

namespace Dargon.Hydar.Grid {
   public interface HydarContext {
      GridConfiguration Configuration { get; }
      Network Network { get; }
      HydarNode Node { get; }
      void SetPhase(IPhase phase);
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
      private IPhase currentPhase;
      private Timer timer;

      public HydarContextImpl(AuditEventBus auditEventBus, GridConfiguration configuration, Network network, NodePhaseFactory phaseFactory, HydarNode node) {
         this.auditEventBus = auditEventBus;
         this.configuration = configuration;
         this.network = network;
         this.phaseFactory = phaseFactory;
         this.node = node;
         this.currentPhase = null;
      }

      public GridConfiguration Configuration { get { return configuration; } }
      public Network Network { get { return network; } }
      public NodePhaseFactory PhaseFactory { get { return phaseFactory; } }
      public HydarNode Node { get { return node; } }

      public void Initialize() {
         currentPhase = phaseFactory.CreateInitializationPhase();
         SetPhase(currentPhase);

         timer = new Timer((e) => {
            lock (synchronization) {
               currentPhase.Tick();
            }
         }, null, configuration.TickIntervalMillis, configuration.TickIntervalMillis);
      }

      public void SetPhase(IPhase phase) {
         lock (synchronization) {
            phase.ThrowIfNull("phase");
            Log("=> " + phase);
            currentPhase = phase;
            currentPhase.Enter();
         }
      }

      public void Receive(IRemoteIdentity senderIdentity, HydarMessage message) {
         if (currentPhase.Process(senderIdentity, message)) {
            // do nothing
         } else {
            auditEventBus.Error(GlobalHydarConfiguration.kAuditEventBusErrorKey, "Unknown Payload Type", "Connectivity Phase: {0}, Payload Type: {1}".F(currentPhase.GetType(), message.Payload.GetType().FullName));
         }
      }

      public void Log(string x) {
         Console.WriteLine(node.Identifier.ToString("n").Substring(0, 8) + " " + x);
      }
   }
}
