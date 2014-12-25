using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Audits;
using Dargon.Hydar.Clustering;

namespace Dargon.Hydar.Caching {
   public interface CacheEpochContextFactory {
      CacheEpochContext Create(EpochDescriptor epochDescriptor);
   }

   public class CacheEpochContextFactoryImpl : CacheEpochContextFactory {
      private readonly AuditEventBus auditEventBus;
      private readonly HydarContext context;

      public CacheEpochContextFactoryImpl(AuditEventBus auditEventBus, HydarContext context) {
         this.auditEventBus = auditEventBus;
         this.context = context;
      }

      public CacheEpochContext Create(EpochDescriptor epochDescriptor) {
         return new CacheEpochContextImpl(auditEventBus, context, epochDescriptor);
      }
   }
}
