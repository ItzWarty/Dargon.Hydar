using Dargon.PortableObjects;
using System;

namespace Dargon.Hydar.PortableObjects {
   public class ElectionAcknowledgement : IPortableObject {
      private Guid acknowledgedVoter;

      public ElectionAcknowledgement() { }

      public ElectionAcknowledgement(Guid acknowledgedVoter) {
         this.acknowledgedVoter = acknowledgedVoter;
      }

      public Guid AcknowledgedVoter { get { return acknowledgedVoter; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, acknowledgedVoter);
      }

      public void Deserialize(IPofReader reader) {
         acknowledgedVoter = reader.ReadGuid(0);
      }
   }
}
