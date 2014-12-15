using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Grid.Phases {
   public interface NodePhaseFactory {
      IPhase CreateIndeterminatePhase();
      IPhase CreateInitializationPhase();
      IPhase CreateElectionPhase();
      IPhase CreateLeaderPhase();
      IPhase CreateMemberPhase();
   }
}
