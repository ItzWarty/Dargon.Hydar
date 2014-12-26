using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Caching {
   public interface CacheEpochContext {
      void HandleNewEpoch();
      void Tick();
      bool Process(IRemoteIdentity sender, HydarMessage<CachingPayload> message);
   }
}
