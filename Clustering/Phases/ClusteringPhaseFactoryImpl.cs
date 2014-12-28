using System;
using Dargon.Audits;
using Dargon.Hydar.Clustering.Messages;
using Dargon.Hydar.Clustering.Phases.Helpers;
using Dargon.Hydar.Networking;
using Dargon.Hydar.Utilities;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering.Phases {
   public class ClusteringPhaseFactoryImpl : ClusteringPhaseFactory {
      private readonly HydarIdentity identity;
      private readonly AuditEventBus auditEventBus;
      private readonly EpochManager epochManager;
      private readonly DebugEventRouter debugEventRouter;
      private readonly OutboundEnvelopeBusImpl outboundEnvelopeBus;
      private readonly ClusteringConfiguration clusteringConfiguration;
      private readonly ClusteringMessageSender clusteringMessageSender;
      private readonly ClusteringPhaseManager clusteringPhaseManager;

      public ClusteringPhaseFactoryImpl(HydarIdentity identity, AuditEventBus auditEventBus, EpochManager epochManager, DebugEventRouter debugEventRouter, OutboundEnvelopeBusImpl outboundEnvelopeBus, ClusteringConfiguration clusteringConfiguration, ClusteringMessageSender clusteringMessageSender, ClusteringPhaseManager clusteringPhaseManager) {
         this.identity = identity;
         this.auditEventBus = auditEventBus;
         this.epochManager = epochManager;
         this.debugEventRouter = debugEventRouter;
         this.outboundEnvelopeBus = outboundEnvelopeBus;
         this.clusteringConfiguration = clusteringConfiguration;
         this.clusteringMessageSender = clusteringMessageSender;
         this.clusteringPhaseManager = clusteringPhaseManager;
      }

      public IPhase CreateInitializationPhase() {
         return new InitializationPhase(clusteringPhaseManager, this);
      }

      public IPhase CreateIndeterminatePhase() {
         return new IndeterminatePhase(clusteringConfiguration, clusteringPhaseManager, this).With(x => x.Initialize());
      }

      public IPhase CreateElectionCandidatePhase(ElectionState electionState, Guid lastEpochId) {
         return new ElectionCandidatePhase(identity, electionState, lastEpochId, debugEventRouter, epochManager, clusteringConfiguration, this, clusteringPhaseManager, clusteringMessageSender).With(x => x.Initialize());
      }

      public IPhase CreateElectionFollowerPhase(ElectionState electionState) {
         return new ElectionFollowerPhase(identity, electionState, epochManager, this, clusteringPhaseManager, clusteringMessageSender).With(x => x.Initialize());
      }

      public IPhase CreateLeaderPhase(IReadOnlySet<Guid> participants) {
         return new LeaderPhase(identity, debugEventRouter, epochManager, clusteringConfiguration, clusteringPhaseManager, this, clusteringMessageSender, participants).With(x => x.Initialize());
      }

      public IPhase CreateFollowerPhase() {
         return new FollowerPhase(debugEventRouter, epochManager, clusteringConfiguration, clusteringPhaseManager, this).With(x => x.Initialize());
      }

      public IPhase CreateDroppedPhase(DateTime epochEndTime) {
         return new DroppedPhase(clusteringPhaseManager, this, epochEndTime).With(x => x.Initialize());
      }
   }
}