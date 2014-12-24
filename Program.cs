using Dargon.Audits;
using Dargon.Hydar.Caching;
using Dargon.Hydar.Clustering;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using Dargon.PortableObjects;
using ItzWarty;
using System;
using System.Threading;

namespace Dargon.Hydar {
   public class Program {
      public static void Main() {
         const int tickIntervalMillis = 200;
         const int ticksToElection = 10;
         const int electionTicksToPromotion = 10;
         const long epochDurationMilliseconds = 20 * 1000;
         IPofContext pofContext = new HydarPofContext();
         IPofSerializer pofSerializer = new PofSerializer(pofContext);
         ClusteringConfiguration configuration = new ClusteringConfigurationImpl(tickIntervalMillis, ticksToElection, electionTicksToPromotion, epochDurationMilliseconds);
         Network network = new TestNetwork(pofSerializer, new TestNetworkConfiguration());
         AuditEventBus auditEventBus = new ConsoleAuditEventBus();
         var hydarFactory = new HydarFactory(configuration, network, auditEventBus);
         Util.Generate(64, i => CreateAndConfigureContext(auditEventBus, hydarFactory));
         for (var i = 0; i < 16; i++) {
            Thread.Sleep((int)epochDurationMilliseconds);
            Util.Generate(8, x => CreateAndConfigureContext(auditEventBus, hydarFactory));
         }
         CountdownEvent synchronization = new CountdownEvent(1);
         synchronization.Wait();
      }

      private static HydarContext CreateAndConfigureContext(AuditEventBus auditEventBus, HydarFactory nodeFactory) {
         var context = nodeFactory.CreateContext();
         var cachingDispatcher = new CachingDispatcher();
         context.RegisterDispatcher(cachingDispatcher);
         var dummyCacheGuid = new Guid(129832, 2189, 19823, 38, 82, 218, 83, 37, 93, 173, 18);
         var dummyCacheConfiguration = new CacheConfigurationImpl { Redundancy = 3 };
         var dummyCacheContext = new CacheContextImpl(auditEventBus, context, dummyCacheGuid, dummyCacheConfiguration);
         dummyCacheContext.Initialize();
         cachingDispatcher.AddCacheContext(dummyCacheContext);
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