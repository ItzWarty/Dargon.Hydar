﻿using Dargon.Audits;
using Dargon.Hydar.Caching;
using Dargon.Hydar.Caching.Data.Partitioning;
using Dargon.Hydar.Clustering;
using Dargon.Hydar.Clustering.Management;
using Dargon.Hydar.Clustering.Messages;
using Dargon.Hydar.Clustering.Phases;
using Dargon.Hydar.Clustering.Utilities;
using Dargon.Hydar.Networking;
using Dargon.Hydar.Networking.Helpers;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Peering;
using Dargon.Hydar.Peering.Management;
using Dargon.Hydar.Peering.Messages;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Proposals;
using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Utilities;
using Dargon.Management;
using Dargon.Management.Server;
using Dargon.PortableObjects;
using ItzWarty;
using ItzWarty.Collections;
using ItzWarty.IO;
using ItzWarty.Networking;
using ItzWarty.Processes;
using ItzWarty.Threading;
using System;
using System.Threading;
using Dargon.Hydar.Caching.Proposals;
using DummyApplication.Management;

namespace DummyApplication {
   public class Program {
      const int TICK_INTERVAL_MILLIS = 200;
      const int TICKS_TO_ELECTION = 10;
      const int ELECTION_TICKS_TO_PROMOTION = 10;
      const long EPOCH_DURATION_MILLISECONDS = 20 * 1000;
      const long PEERING_MAXIMUM_HEARTBEAT_INTERVAL = 20 * 10000;

      public static void Main() {
         IPofContext pofContext = new PofContext().With(x => {
            x.MergeContext(new HydarPofContext());
            x.MergeContext(new HydarUtilitiesPofContext());
            x.MergeContext(new HydarNetworkingPofContext());
            x.MergeContext(new HydarClusteringPofContext());
            x.MergeContext(new HydarCachingImplPofContext());
            x.MergeContext(new HydarAtomicProposalsPofContext());
            x.MergeContext(new ManagementPofContext());
            x.MergeContext(new DummyApplicationPofContext());
         });
         IPofSerializer pofSerializer = new PofSerializer(pofContext);
         ClusteringConfiguration configuration = new ClusteringConfigurationImpl(TICKS_TO_ELECTION, ELECTION_TICKS_TO_PROMOTION, EPOCH_DURATION_MILLISECONDS);
         AuditEventBus auditEventBus = new ConsoleAuditEventBus();
         TestNetworkManager testNetworkManager = new TestNetworkManager(pofSerializer, new TestNetworkConfiguration());
         Util.Generate(16, i => CreateAndConfigureContext(pofContext, pofSerializer, auditEventBus, testNetworkManager));
         //Util.Generate(64, i => CreateAndConfigureContext(auditEventBus, testNetworkManager));
         // for (var i = 0; i < 8; i++) {
         //    Thread.Sleep((int)EPOCH_DURATION_MILLISECONDS);
         //    Util.Generate(12, x => CreateAndConfigureContext(auditEventBus, testNetworkManager));
         // }
         CountdownEvent synchronization = new CountdownEvent(1);
         synchronization.Wait();
      }

      private static int kas = 0;
      private static object CreateAndConfigureContext(IPofContext pofContext, IPofSerializer pofSerializer, AuditEventBus auditEventBus, TestNetworkManager testNetworkManager) {
         var testNodeNumber = (kas++);

         // construct libwarty dependencies
         ICollectionFactory collectionFactory = new CollectionFactory();

         // construct libwarty-proxies dependencies
         IStreamFactory streamFactory = new StreamFactory();
         IFileSystemProxy fileSystemProxy = new FileSystemProxy(streamFactory);
         IThreadingFactory threadingFactory = new ThreadingFactory();
         ISynchronizationFactory synchronizationFactory = new SynchronizationFactory();
         IThreadingProxy threadingProxy = new ThreadingProxy(threadingFactory, synchronizationFactory);
         IDnsProxy dnsProxy = new DnsProxy();
         ITcpEndPointFactory tcpEndPointFactory = new TcpEndPointFactory(dnsProxy);
         INetworkingInternalFactory networkingInternalFactory = new NetworkingInternalFactory(threadingProxy, streamFactory);
         ISocketFactory socketFactory = new SocketFactory(tcpEndPointFactory, networkingInternalFactory);
         INetworkingProxy networkingProxy = new NetworkingProxy(socketFactory, tcpEndPointFactory);
         IProcessProxy processProxy = new ProcessProxy();

         // construct libdargon.management dependencies
         ITcpEndPoint managementServerEndpoint = networkingProxy.CreateAnyEndPoint(41000 + testNodeNumber);
         IMessageFactory managementServerMessageFactory = new MessageFactory();
         IManagementSessionFactory managementSessionFactory = new ManagementSessionFactory(collectionFactory, threadingProxy, pofSerializer, managementServerMessageFactory);
         IManagementContextFactory managementContextFactory = new ManagementContextFactory(pofContext);
         ILocalManagementServerContext localManagementServerContext = new LocalManagementServerContext(collectionFactory, managementSessionFactory);
         ILocalManagementRegistry localManagementServerRegistry = new LocalManagementRegistry(pofSerializer, managementContextFactory, localManagementServerContext);
         IManagementServerConfiguration localManagementServerConfiguration = new ManagementServerConfiguration(managementServerEndpoint);
         var localManagementServer = new LocalManagementServer(threadingProxy, networkingProxy, managementSessionFactory, localManagementServerContext, localManagementServerConfiguration);
         localManagementServer.Initialize();

         // Initialize Hydar Base Dependencies
         var tickerConfiguration = new PeriodicTickerConfigurationImpl(TICK_INTERVAL_MILLIS);
         var ticker = new PeriodicTickerImpl(threadingProxy, tickerConfiguration);
         var nodeId = new Guid(testNodeNumber, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0); //Guid.NewGuid();
         var hydarIdentity = new HydarIdentityImpl(nodeId);
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
         var clusteringPhaseManager = new ClusteringPhaseManagerImpl(hydarIdentity, ticker, debugEventRouter, inboundEnvelopeBus);
         var clusteringMessageSender = new ClusteringMessageSenderImpl(outboundEnvelopeFactory, outboundEnvelopeBus, clusteringMessageFactory);
         var clusteringConfiguration = new ClusteringConfigurationImpl(TICKS_TO_ELECTION, ELECTION_TICKS_TO_PROMOTION, EPOCH_DURATION_MILLISECONDS);
         var clusteringPhaseFactory = new ClusteringPhaseFactoryImpl(hydarIdentity, auditEventBus, epochManager, debugEventRouter, outboundEnvelopeBus, clusteringConfiguration, clusteringMessageSender, clusteringPhaseManager);
         clusteringPhaseManager.Initialize();
         clusteringPhaseManager.Transition(clusteringPhaseFactory.CreateInitializationPhase());

         // Initialize Peering Dependencies
         var peeringConfiguration = new PeeringConfigurationImpl(PEERING_MAXIMUM_HEARTBEAT_INTERVAL);
         var peerStatusFactory = new PeerStatusFactoryImpl(peeringConfiguration);
         var manageablePeeringState = new PeeringStateImpl(peerStatusFactory);
         var peeringMessageDispatcher = new PeeringMessageDispatcherImpl(inboundEnvelopeBus, manageablePeeringState).With(x => x.Initialize());
         var peeringMessageFactory = new PeeringMessageFactoryImpl();
         var peeringMessageSender = new PeeringMessageSenderImpl(outboundEnvelopeFactory, outboundEnvelopeBus, peeringMessageFactory);
         var peeringAnnouncementBroadcaster = new PeeringAnnouncementBroadcasterImpl(ticker, peeringMessageSender).With(x => x.Initialize());
         localManagementServerRegistry.RegisterInstance(new PeeringDebugMob(manageablePeeringState));

         // management
         localManagementServerRegistry.RegisterInstance(new ClusteringManagementMob(hydarIdentity, epochManager));

         // Initialize Caching Subsystem Dependencies
         var dummyCache = "Dummy Cache";
         var cacheId = Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");
         var partitioningStrategy = new UnweightedRingHashSpacePartitioningStrategy(1024, 3);
         var cacheAtomicExecutionContext = new CacheAtomicExecutionContext<int>();
         var subjectStateFactory = new SubjectStateFactoryImpl<int>(cacheAtomicExecutionContext);
         var subjectStateManager = new SubjectStateManagerImpl<int>(subjectStateFactory);
         var atomicProposalMessageFactory = new AtomicProposalMessageFactoryImpl<int>(cacheId);
         var proposalMessageSender = new ProposalMessageSenderImpl<int>(cacheId, outboundEnvelopeFactory, outboundEnvelopeBus, atomicProposalMessageFactory);
         var proposalStateFactory = new ProposalStateFactoryImpl<int>(cacheId, subjectStateManager, proposalMessageSender);
         var proposalStateManager = new ProposalStateManagerImpl<int>(proposalStateFactory);
         var inboundEnvelopeChannel = new InboundEnvelopeChannelImpl(threadingProxy);
         subjectStateFactory.SetProposalStateManager(proposalStateManager);
         var atomicProposalEnvelopeChannelConsumer = new AtomicProposalEnvelopeChannelConsumerImpl<int>(threadingProxy, hydarIdentity, inboundEnvelopeChannel, proposalStateManager, cacheId);
         var atomicProposalManagementService = new AtomicProposalManagementServiceImpl<int>(subjectStateManager);
         var cacheProposalFactory = new CacheProposalFactoryImpl<int, string>(threadingProxy);
         var cacheService = new HydarCacheServiceImpl<int, string>(atomicProposalManagementService, cacheProposalFactory);
         localManagementServerRegistry.RegisterInstance(new DummyCacheServiceDebugMob<int, string>(cacheService));

         //         var activeProposalRegistry = new ActiveProposalManagerImpl<int, string>();
         //         var proposalPhaseFactory = new ProposalPhaseFactoryImpl<int, string>(activeProposalRegistry);
         //         var proposalContextFactory = new ProposalContextFactoryImpl<int, string>(proposalPhaseFactory);
         //         var proposalInboundEnvelopeChannel = new InboundEnvelopeChannelImpl(threadingProxy);
         //         var proposalManager = new TopicEnvelopeDispatcherImpl<int, string>(threadingProxy, hydarIdentity, proposalInboundEnvelopeChannel, proposalContextFactory, cacheGuid);
         //         var proposalTopicEnvelopeChannel = new InboundEnvelopeChannelImpl(threadingProxy);
         //         var cacheDispatcher = new CacheDispatcherImpl<int, string>(dummyCache, cacheGuid, hydarIdentity, proposalTopicEnvelopeChannel);
         // var cacheManager = new OldCacheEnvelopeDispatcherImpl(hydarIdentity, inboundEnvelopeBus).With(x => x.Initialize());
         // var dummyCacheContext = new CacheDispatcherImpl("Dummy Cache", Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"));
         // var dummyCacheBlockContainer = new BlockContainerImpl<int, string>(partitioningStrategy);
         // var dummyCacheOperationManager = new CacheOperationManagerImpl<int, string>(partitioningStrategy, dummyCacheBlockContainer);
         // cacheManager.RegisterCache(dummyCacheContext);
         // localManagementServerRegistry.RegisterInstance(new DummyCacheDebugMob(dummyCacheOperationManager, dummyCacheBlockContainer));

         ticker.Initialize();
         return null;
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