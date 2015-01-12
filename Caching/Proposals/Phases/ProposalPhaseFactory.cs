using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public interface ProposalPhaseFactory<K, V> {
      IProposalPhase<K, V> Initial(ProposalContext<K, V> proposalContext);
      IProposalPhase<K, V> PreparedPhase(ProposalContext<K, V> proposalContext);
   }

   public class ProposalPhaseFactoryImpl<K, V> : ProposalPhaseFactory<K, V> {
      public IProposalPhase<K, V> Initial(ProposalContext<K, V> proposalContext) {
         return new InitialPhase<K, V>(proposalContext, this);
      }

      public IProposalPhase<K, V> PreparedPhase(ProposalContext<K, V> proposalContext) {
         return new PreparedPhase<K, V>(proposalContext, this);
      }
   }
}
