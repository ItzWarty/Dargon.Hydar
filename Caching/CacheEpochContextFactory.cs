using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Audits;
using Dargon.Hydar.Clustering;
using ItzWarty;

namespace Dargon.Hydar.Caching {
   public interface CacheEpochContextFactory {
      CacheEpochContext Create(CacheContext cacheContext, EpochDescriptor epochDescriptor);
   }

   public class CacheEpochContextFactoryImpl : CacheEpochContextFactory {
      private readonly AuditEventBus auditEventBus;
      private readonly RootMessageDispatcher rootMessageDispatcher;

      public CacheEpochContextFactoryImpl(AuditEventBus auditEventBus, RootMessageDispatcher rootMessageDispatcher) {
         this.auditEventBus = auditEventBus;
         this.rootMessageDispatcher = rootMessageDispatcher;
      }

      public CacheEpochContext Create(CacheContext cacheContext, EpochDescriptor epochDescriptor) {
         return new CacheEpochContextImpl(auditEventBus, rootMessageDispatcher, cacheContext, epochDescriptor).With(x => x.Initialize());
      }
   }
}
