using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Caching {
   public interface CacheContext {
      Guid Id { get; }

      bool Process(IRemoteIdentity sender, HydarMessage<CachingPayload> message);
   }

   public class CacheContextImpl : MessageProcessorBase<HydarMessage<CachingPayload>, Action<IRemoteIdentity, HydarMessageHeader, CachingPayload>>, CacheContext {
      private readonly Guid cacheId;
      private readonly CacheConfiguration configuration;

      public CacheContextImpl(AuditEventBus auditEventBus, HydarContext hydarContext, Guid cacheId, CacheConfiguration configuration)
         : base(auditEventBus, hydarContext) {
         this.cacheId = cacheId;
         this.configuration = configuration;
      }

      public Guid Id { get { return cacheId; } }

      protected override void Invoke(Action<IRemoteIdentity, HydarMessageHeader, CachingPayload> handler, IRemoteIdentity sender, HydarMessage<CachingPayload> message) {
         handler(sender, message.Header, message.Payload);
      }

      protected void SendGeneric<TPayload>(TPayload payload) {
         var header = new HydarMessageHeaderImpl(node.Identifier);
         network.Broadcast(node, new HydarMessageImpl<TPayload>(header, payload));
      }

      protected void SendCache<TPayload>(TPayload innerPayload) {
         var metadata = new CachingPayloadMetadata(cacheId);
         SendGeneric(new CachingPayload<TPayload>(metadata, innerPayload));
      }
   }
}
