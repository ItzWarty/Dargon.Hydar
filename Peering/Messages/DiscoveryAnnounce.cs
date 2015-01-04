using System;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Peering.Messages {
   public class DiscoveryAnnounce : IPortableObject {
      private Guid nodeIdentifier;

      public DiscoveryAnnounce(Guid nodeIdentifier) {
         this.nodeIdentifier = nodeIdentifier;
      }

      public Guid NodeIdentifier { get { return nodeIdentifier; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, nodeIdentifier);
      }

      public void Deserialize(IPofReader reader) {
         nodeIdentifier = reader.ReadGuid(0);
      }
   }
}
