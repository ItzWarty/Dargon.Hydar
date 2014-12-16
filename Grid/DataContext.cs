using Dargon.Audits;
using Dargon.Hydar.Grid.ClusterPhases;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Grid {
   public interface DataContext {
      bool Process(IRemoteIdentity sender, HydarMessage message);
   }

   public class DataContextImpl : MessageProcessorBase,  DataContext {
      public DataContextImpl(
         AuditEventBus auditEventBus, 
         NodePhaseFactory phaseFactory, 
         HydarContext context
      ) : base(auditEventBus, phaseFactory, context) {
      }
   }
}
