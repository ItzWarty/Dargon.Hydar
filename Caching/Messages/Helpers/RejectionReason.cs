using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Caching.Messages.Helpers {
   public enum RejectionReason : uint {
      /// <summary>
      /// The proposal has been bullied out by one with a higher identifier.
      /// </summary>
      Bullied
   }
}
