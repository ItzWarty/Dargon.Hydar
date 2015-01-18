using ItzWarty;

namespace Dargon.Hydar.Proposals.Phases {
   public interface ProposalPhaseFactory<K, V> {
      IProposalPhase<K, V> Initial(ProposalContext<K, V> proposalContext);
      IProposalPhase<K, V> AcceptedPhase(ProposalContext<K, V> proposalContext);
      IProposalPhase<K, V> RejectedPhase(ProposalContext<K, V> proposalContext);
   }

   public class ProposalPhaseFactoryImpl<K, V> : ProposalPhaseFactory<K, V> {
      private readonly ActiveProposalManager<K, V> activeProposalManager;

      public ProposalPhaseFactoryImpl(ActiveProposalManager<K, V> activeProposalManager) {
         this.activeProposalManager = activeProposalManager;
      }

      public IProposalPhase<K, V> Initial(ProposalContext<K, V> proposalContext) {
         return new InitialPhase<K, V>(proposalContext, this, activeProposalManager);
      }

      public IProposalPhase<K, V> AcceptedPhase(ProposalContext<K, V> proposalContext) {
         return new FollowerAcceptedPhase<K, V>(proposalContext, this).With(x => x.Initialize());
      }

      public IProposalPhase<K, V> RejectedPhase(ProposalContext<K, V> proposalContext) {
         return new FollowerRejectedPhase<K, V>(proposalContext).With(x => x.Initialize());
      }
   }
}
