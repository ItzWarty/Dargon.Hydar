using System;
using Dargon.Audits;
using Dargon.Hydar.Clustering;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Caching {
   public class CacheEpochContextImpl : MessageProcessorBase<HydarMessage<CachingPayload>, Action<IRemoteIdentity, HydarMessageHeader, CachingPayload>>, CacheEpochContext {
      protected readonly EpochDescriptor epoch;

      public CacheEpochContextImpl(
         AuditEventBus auditEventBus, 
         HydarContext context, 
         EpochDescriptor epoch
         ) : base(auditEventBus, context) {
         this.epoch = epoch;
      }

      public void Tick() {

      }

      protected override void Invoke(Action<IRemoteIdentity, HydarMessageHeader, CachingPayload> handler, IRemoteIdentity sender, HydarMessage<CachingPayload> message) {
         handler(sender, message.Header, message.Payload);
      }
   }
}