using Dargon.Audits;
using Dargon.Hydar.Clustering;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;
using System;
using System.Collections.Generic;

namespace Dargon.Hydar.Caching {
   public interface CacheContext {
      Guid Id { get; }
      CacheConfiguration Configuration { get; }

      void Tick();
      bool Process(IRemoteIdentity sender, HydarMessage<CachingPayload> message);
   }

   public class CacheContextImpl : MessageProcessorBase<HydarMessage<CachingPayload>, Action<IRemoteIdentity, HydarMessageHeader, CachingPayload>>, CacheContext {
      private readonly Guid cacheId;
      private readonly CacheConfiguration defaultCacheConfiguration;
      private readonly CacheEpochContextFactory epochContextFactory;
      private readonly ClusterContext clusterContext;
      private readonly Dictionary<Guid, CacheEpochContext> contextsByEpochId = new Dictionary<Guid, CacheEpochContext>();
      private readonly object synchronization = new object();

      public CacheContextImpl(
         AuditEventBus auditEventBus, 
         HydarContext hydarContext, 
         Guid cacheId, 
         CacheConfiguration defaultCacheConfiguration, 
         CacheEpochContextFactory epochContextFactory
      ) : base(auditEventBus, hydarContext) {
         this.cacheId = cacheId;
         this.defaultCacheConfiguration = defaultCacheConfiguration;
         this.epochContextFactory = epochContextFactory;

         this.clusterContext = context.ClusterContext;
      }

      public Guid Id { get { return cacheId; } }
      public CacheConfiguration Configuration { get { return defaultCacheConfiguration; } }

      public void Initialize() {
         clusterContext.NewEpoch += HandleNewEpoch;
      }

      private void HandleNewEpoch(EpochDescriptor epoch) {
         Log("New Epoch " + epoch.Id);
         var epochContext = epochContextFactory.Create(this, epoch);
         contextsByEpochId.Add(epoch.Id, epochContext);
         epochContext.HandleNewEpoch();
      }

      public void Tick() {
         foreach (var epochContext in contextsByEpochId.Values) {
            epochContext.Tick();
         }
      }

      public override bool Process(IRemoteIdentity sender, HydarMessage<CachingPayload> message) {
         if (base.Process(sender, message)) {
            return true;
         }
         var payload = message.Payload;
         var metadata = payload.Metadata;
         CacheEpochContext epochContext;
         if (!contextsByEpochId.TryGetValue(metadata.EpochId, out epochContext)) {
            return false;
         } else {
            return epochContext.Process(sender, message);
         }
      }

      protected override void Invoke(Action<IRemoteIdentity, HydarMessageHeader, CachingPayload> handler, IRemoteIdentity sender, HydarMessage<CachingPayload> message) {
         handler(sender, message.Header, message.Payload);
      }

      protected override object GetPayload(HydarMessage<CachingPayload> message) {
         return message.Payload.InnerPayload;
      }

      protected void SendGeneric<TPayload>(TPayload payload) {
         var header = new HydarMessageHeaderImpl(node.Identifier);
         network.Broadcast(node, new HydarMessageImpl<TPayload>(header, payload));
      }

      protected void SendCache<TPayload>(TPayload innerPayload) {
         var metadata = new CachingPayloadMetadata(cacheId, clusterContext.GetCurrentEpoch().Id);
         SendGeneric(new CachingPayload<TPayload>(metadata, innerPayload));
      }
   }
}
