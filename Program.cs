using Dargon.Audits;
using Dargon.Hydar.Clustering;
using Dargon.Hydar.Clustering.Phases;
using Dargon.Hydar.Networking;
using Dargon.Hydar.Networking.Helpers;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;
using Dargon.PortableObjects;
using ItzWarty;
using System;
using System.Threading;
using Dargon.Hydar.Clustering.Messages;
using Dargon.Hydar.Networking.PortableObjects;

namespace Dargon.Hydar {
   public class Program {
      const int TICK_INTERVAL_MILLIS = 200;
      const int TICKS_TO_ELECTION = 10;
      const int ELECTION_TICKS_TO_PROMOTION = 10;
      const long EPOCH_DURATION_MILLISECONDS = 20 * 1000;

      public static void Main() {
         IPofContext pofContext = new HydarPofContext();
         IPofSerializer pofSerializer = new PofSerializer(pofContext);
         ClusteringConfiguration configuration = new ClusteringConfigurationImpl(TICKS_TO_ELECTION, ELECTION_TICKS_TO_PROMOTION, EPOCH_DURATION_MILLISECONDS);
         AuditEventBus auditEventBus = new ConsoleAuditEventBus();
         TestNetworkManager testNetworkManager = new TestNetworkManager(pofSerializer, new TestNetworkConfiguration());
         Util.Generate(32, i => CreateAndConfigureContext(auditEventBus, testNetworkManager));
         //Util.Generate(64, i => CreateAndConfigureContext(auditEventBus, testNetworkManager));
         // for (var i = 0; i < 8; i++) {
         //    Thread.Sleep((int)EPOCH_DURATION_MILLISECONDS);
         //    Util.Generate(12, x => CreateAndConfigureContext(auditEventBus, testNetworkManager));
         // }
         CountdownEvent synchronization = new CountdownEvent(1);
         synchronization.Wait();
      }

      private static int kas = 0;
      private static object CreateAndConfigureContext(AuditEventBus auditEventBus, TestNetworkManager testNetworkManager) {
         // Initialize Hydar Base Dependencies
         var nodeId = new Guid(kas++, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0); //Guid.NewGuid();
         var hydarIdentity = new HydarIdentityImpl(nodeId);
         var hydarConfiguration = new HydarConfigurationImpl(TICK_INTERVAL_MILLIS);
         var debugEventRouter = new DebugEventRouterImpl(hydarIdentity, auditEventBus);
         var network = testNetworkManager.CreateNetworkInstance().With(testNetworkManager.Join);
         var inboundEnvelopeBus = new InboundEnvelopeBusImpl();
         var networkToInboundEnvelopeBusLink = new FilteredNetworkToInboundBusLink(network, inboundEnvelopeBus, hydarIdentity).With(x => x.Initialize());
         var outboundEnvelopeBus = new OutboundEnvelopeBusImpl();
         var outboundEnvelopeBusToNetworkLink = new OutboundBusToNetworkLink(outboundEnvelopeBus, network).With(x => x.Initialize());
         var outboundEnvelopeFactory = new OutboundEnvelopeFactoryImpl(hydarIdentity);

         // Initialize Clustering Subsystem Dependencies
         var epochManager = new EpochManagerImpl().With(x => x.Initialize());
         var clusteringMessageFactory = new ClusteringMessageFactoryImpl();
         var clusteringPhaseManager = new ClusteringPhaseManagerImpl(debugEventRouter, inboundEnvelopeBus);
         var clusteringMessageSender = new ClusteringMessageSenderImpl(outboundEnvelopeFactory, outboundEnvelopeBus, clusteringMessageFactory);
         var clusteringConfiguration = new ClusteringConfigurationImpl(TICKS_TO_ELECTION, ELECTION_TICKS_TO_PROMOTION, EPOCH_DURATION_MILLISECONDS);
         var clusteringPhaseFactory = new ClusteringPhaseFactoryImpl(hydarIdentity, auditEventBus, epochManager, debugEventRouter, outboundEnvelopeBus, clusteringConfiguration, clusteringMessageSender, clusteringPhaseManager);
         clusteringPhaseManager.Initialize();
         clusteringPhaseManager.Transition(clusteringPhaseFactory.CreateInitializationPhase());
         new Thread(() => {
            while(true) {
               clusteringPhaseManager.Tick();
               Thread.Sleep(hydarConfiguration.TickIntervalMillis);
            }
         }).Start();
         return null;
         // var dummyCacheGuid = new Guid(129832, 2189, 19823, 38, 82, 218, 83, 37, 93, 173, 18);
         // var dummyCacheConfiguration = new CacheConfigurationImpl { Redundancy = 3 };
         // var cacheEpochDispatcherFactory = new CacheEpochContextFactoryImpl(auditEventBus, context);
         // var dummyCacheContext = new CacheContextImpl(auditEventBus, context, dummyCacheGuid, dummyCacheConfiguration, cacheEpochDispatcherFactory);
         // dummyCacheContext.Initialize();
         // cachingDispatcher.AddCacheContext(dummyCacheContext);
         // return context;
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