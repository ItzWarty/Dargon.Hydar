using System;
using System.Threading;
using Dargon.Audits;
using Dargon.Hydar.Caching;
using Dargon.Hydar.Clustering;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using Dargon.PortableObjects;
using ItzWarty;

namespace Dargon.Hydar {
   public class Program {
      public static void Main() {
         const int tickIntervalMillis = 500;
         const int ticksToElection = 10;
         const int electionTicksToPromotion = 10;
         IPofContext pofContext = new HydarPofContext();
         IPofSerializer pofSerializer = new PofSerializer(pofContext);
         ClusteringConfiguration configuration = new ClusteringConfigurationImpl(tickIntervalMillis, ticksToElection, electionTicksToPromotion);
         Network network = new TestNetwork(pofSerializer, new TestNetworkConfiguration());
         AuditEventBus auditEventBus = new ConsoleAuditEventBus();
         var hydarFactory = new HydarFactory(configuration, network, auditEventBus);
         var context1 = CreateAndConfigureContext(hydarFactory);
         var context2 = CreateAndConfigureContext(hydarFactory);
         var context3 = CreateAndConfigureContext(hydarFactory);
         var context4 = CreateAndConfigureContext(hydarFactory);

         CountdownEvent synchronization = new CountdownEvent(1);
         synchronization.Wait();
      }

      private static HydarContext CreateAndConfigureContext(HydarFactory nodeFactory) {
         var context = nodeFactory.CreateContext();
         var cachingDispatcher = new CachingDispatcher();
         context.RegisterDispatcher(cachingDispatcher);

         return context;
      }
   }

   public class ConsoleAuditEventBus : AuditEventBus {
      public event EventBusHandler<AuditEvent> EventPosted;

      public void Post(AuditEvent obj) {
         Console.WriteLine("{0}: {1} // {2}".F(obj.EventType, obj.EventMessage, obj.EventData));

         var capture = EventPosted;
         if (capture != null) {
            capture(this, obj);
         }
      }

      public void Post(AuditEventType eventType, string eventKey, string message, string data = null) {
         Post(new AuditEvent(eventType, eventKey, message, data));
      }
   }
}