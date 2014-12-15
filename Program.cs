using System;
using System.Threading;
using Dargon.Audits;
using Dargon.Hydar.Grid;
using Dargon.Hydar.Networking;
using ItzWarty;

namespace Dargon.Hydar {
   public class Program {
      public static void Main() {
         const int tickIntervalMillis = 500;
         const int ticksToElection = 10;
         const int electionTicksToPromotion = 10;
         GridConfiguration configuration = new GridConfigurationImpl(tickIntervalMillis, ticksToElection, electionTicksToPromotion);
         Network network = new TestNetwork(new TestNetworkConfiguration());
         AuditEventBus auditEventBus = new ConsoleAuditEventBus();
         var nodeFactory = new HydarNodeFactory(configuration, network, auditEventBus);
         var node1 = nodeFactory.Create();
         var node2 = nodeFactory.Create();
         var node3 = nodeFactory.Create();
         var node4 = nodeFactory.Create();
         
         CountdownEvent synchronization = new CountdownEvent(1);
         synchronization.Wait();
      }
   }

   public class ConsoleAuditEventBus : AuditEventBus {
      public event EventBusHandler<AuditEvent> EventPosted;

      public void Post(AuditEvent obj) {
         Console.WriteLine("{0}: {1} // {2}".F(obj.EventType, obj.EventMessage, obj.EventData));
      }

      public void Post(AuditEventType eventType, string eventKey, string message, string data = null) {
         Post(new AuditEvent(eventType, eventKey, message, data));
      }
   }
}