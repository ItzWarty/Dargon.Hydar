using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar {
   public interface HydarSubsystem : HydarDispatcher {
      void Tick();
   }
}
