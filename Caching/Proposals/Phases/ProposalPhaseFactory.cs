using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItzWarty;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public interface ProposalPhaseFactory<K, V> {
      IProposalPhase Initial(ProposalContext<K, V> proposalContext);
      IProposalPhase AcceptedPhase(ProposalContext<K, V> proposalContext);
      IProposalPhase RejectedPhase(ProposalContext<K, V> proposalContext);
   }

   public class ProposalPhaseFactoryImpl<K, V> : ProposalPhaseFactory<K, V> {
      private readonly ActiveProposalRegistry<K, V> activeProposalRegistry;

      public ProposalPhaseFactoryImpl(ActiveProposalRegistry<K, V> activeProposalRegistry) {
         this.activeProposalRegistry = activeProposalRegistry;
      }

      public IProposalPhase Initial(ProposalContext<K, V> proposalContext) {
         return new InitialPhase<K, V>(proposalContext, this, activeProposalRegistry);
      }

      public IProposalPhase AcceptedPhase(ProposalContext<K, V> proposalContext) {
         return new FollowerAcceptedPhase<K, V>(proposalContext, this).With(x => x.Initialize());
      }

      public IProposalPhase RejectedPhase(ProposalContext<K, V> proposalContext) {
         return new FollowerRejectedPhase<K, V>(proposalContext).With(x => x.Initialize());
      }
   }
}
