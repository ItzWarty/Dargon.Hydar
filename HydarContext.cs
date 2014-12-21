using Dargon.Hydar.Clustering;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar {
   public interface HydarContext {
      ClusteringConfiguration Configuration { get; }
      Network Network { get; }
      NetworkNode Node { get; }
      ClusterContext ClusterContext { get; }

      void Dispatch(IRemoteIdentity senderIdentity, HydarMessage message);
      void Log(string x);
   }
}
