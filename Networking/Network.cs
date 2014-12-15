using Dargon.Hydar.Grid;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Networking {
   public interface Network {
      void Join(HydarNode node);
      void Part(HydarNode node);
      void Broadcast(HydarNode sender, HydarMessage message);
   }
}