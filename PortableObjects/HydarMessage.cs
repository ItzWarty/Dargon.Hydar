using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public interface HydarMessage : IPortableObject {
      HydarMessageHeader Header { get; }
      object Payload { get; }
   }

   public interface HydarMessage<TPayload> : HydarMessage {
      new TPayload Payload { get; }
   }
}
