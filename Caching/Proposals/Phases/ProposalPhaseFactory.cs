using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public interface ProposalPhaseFactory<K, V> {
      IProposalPhase Initial(ProposalContext<K, V> proposalContext);
      IProposalPhase PreparedPhase(ProposalContext<K, V> proposalContext);
   }

   public class ProposalPhaseFactoryImpl<K, V> : ProposalPhaseFactory<K, V> {
      public IProposalPhase Initial(ProposalContext<K, V> proposalContext) {
         return new InitialPhase<K, V>(proposalContext, this);
      }

      public IProposalPhase PreparedPhase(ProposalContext<K, V> proposalContext) {
         return new PreparedPhase<K, V>(proposalContext, this);
      }
   }
}
