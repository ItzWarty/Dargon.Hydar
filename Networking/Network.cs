using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Networking {
   public interface Network {
      void Join(NetworkNode node);
      void Part(NetworkNode node);
      void Broadcast(NetworkNode sender, HydarMessage message);
   }
}