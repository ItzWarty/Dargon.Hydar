using ItzWarty;

namespace Dargon.Hydar.Proposals.Phases {
   public interface ProposalPhaseFactory<K, V> {
      IProposalPhase<K, V> Initial(ProposalContext<K, V> proposalContext);
      IProposalPhase<K, V> AcceptedPhase(ProposalContext<K, V> proposalContext);
      IProposalPhase<K, V> RejectedPhase(ProposalContext<K, V> proposalContext);
   }

   public class ProposalPhaseFactoryImpl<K, V> : ProposalPhaseFactory<K, V> {
      private readonly ActiveProposalRegistry<K, V> activeProposalRegistry;

      public ProposalPhaseFactoryImpl(ActiveProposalRegistry<K, V> activeProposalRegistry) {
         this.activeProposalRegistry = activeProposalRegistry;
      }

      public IProposalPhase<K, V> Initial(ProposalContext<K, V> proposalContext) {
         return new InitialPhase<K, V>(proposalContext, this, activeProposalRegistry);
      }

      public IProposalPhase<K, V> AcceptedPhase(ProposalContext<K, V> proposalContext) {
         return new FollowerAcceptedPhase<K, V>(proposalContext, this).With(x => x.Initialize());
      }

      public IProposalPhase<K, V> RejectedPhase(ProposalContext<K, V> proposalContext) {
         return new FollowerRejectedPhase<K, V>(proposalContext).With(x => x.Initialize());
      }
   }
}
