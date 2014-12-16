using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Grid.Data {
   public interface DataContext {
      bool Process(IRemoteIdentity sender, HydarMessage message);
      void Tick();
   }

   public class DataContextImpl : MessageProcessorBase, DataContext {
      public DataContextImpl(
         AuditEventBus auditEventBus, 
         HydarContext context
      ) : base(auditEventBus, context) {
      }

      public void Tick() {

      }
   }
}
