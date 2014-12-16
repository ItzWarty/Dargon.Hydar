using Dargon.Audits;
using Dargon.Hydar.Grid.Data;
using Dargon.Hydar.Grid.Peering;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;
using System;
using System.Threading;

namespace Dargon.Hydar.Grid {
   public interface HydarContext {
      GridConfiguration Configuration { get; }
      Network Network { get; }
      HydarNode Node { get; }
      PeeringContext PeeringContext { get; }
      DataContext DataContext { get; }

      void Receive(IRemoteIdentity senderIdentity, HydarMessage message);
      void Log(string x);
   }

   public class HydarContextImpl : HydarContext {
      private readonly AuditEventBus auditEventBus;
      private readonly GridConfiguration configuration;
      private readonly Network network;
      private readonly HydarNode node;
      private PeeringContext peeringContext;
      private DataContext dataContext;
      private Timer timer;

      public HydarContextImpl(AuditEventBus auditEventBus, GridConfiguration configuration, Network network, HydarNode node) {
         this.auditEventBus = auditEventBus;
         this.configuration = configuration;
         this.network = network;
         this.node = node;
      }

      public GridConfiguration Configuration { get { return configuration; } }
      public Network Network { get { return network; } }
      public HydarNode Node { get { return node; } }
      public PeeringContext PeeringContext { get { return peeringContext; } }
      public DataContext DataContext { get { return dataContext; } }

      public void SetPeeringContext(PeeringContext peeringContext) {
         this.peeringContext = peeringContext;
      }

      public void SetDataContext(DataContext dataContext) {
         this.dataContext = dataContext;
      }

      public void Initialize() {
         timer = new Timer((x) => {
            peeringContext.Tick();
            dataContext.Tick();
         }, null, configuration.TickIntervalMillis, configuration.TickIntervalMillis);
      }

      public void Receive(IRemoteIdentity senderIdentity, HydarMessage message) {
         if (peeringContext.Process(senderIdentity, message)) {
            // do nothing
         } else if (dataContext.Process(senderIdentity, message)) {
            // do nothing
         } else {
            auditEventBus.Error(GlobalHydarConfiguration.kAuditEventBusErrorKey, "Unknown Payload Type", "Connectivity Phase: {0}, Payload Type: {1}".F(PeeringContext.GetCurrentPhase().GetType(), message.Payload.GetType().FullName));
         }
      }

      public void Log(string x) {
         Console.WriteLine(node.Identifier.ToString("n").Substring(0, 8) + " " + x);
      }
   }
}
