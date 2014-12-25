using Dargon.Audits;
using Dargon.Hydar.Clustering;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;
using ItzWarty.Collections;
using System;
using SCG = System.Collections.Generic;

namespace Dargon.Hydar.Caching {
   public class CacheEpochContextImpl : MessageProcessorBase<HydarMessage<CachingPayload>, Action<IRemoteIdentity, HydarMessageHeader, CachingPayload>>, CacheEpochContext {
      private readonly CacheContext cacheContext;
      private readonly EpochDescriptor epochDesciptor;
      private readonly CacheConfiguration configuration;
      private SCG.IReadOnlyDictionary<Guid, IReadOnlySet<PartitionRange>> partitionRangesByNodeId;

      public CacheEpochContextImpl(
         AuditEventBus auditEventBus, 
         HydarContext context, 
         CacheContext cacheContext, 
         EpochDescriptor epochDesciptor
      ) : base(auditEventBus, context) {
         this.cacheContext = cacheContext;
         this.epochDesciptor = epochDesciptor;

         this.configuration = cacheContext.Configuration;
      }

      public void Initialize() {
         partitionRangesByNodeId = configuration.PartitioningStrategy.Partition(epochDesciptor.ParticipantGuids, configuration.BlockCount, configuration.Redundancy);
      }

      public void Tick() {
      }

      protected override void Invoke(Action<IRemoteIdentity, HydarMessageHeader, CachingPayload> handler, IRemoteIdentity sender, HydarMessage<CachingPayload> message) {
         handler(sender, message.Header, message.Payload);
      }
   }
}