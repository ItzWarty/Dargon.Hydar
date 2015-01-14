using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Phases;

namespace Dargon.Hydar.Proposals {
   public interface ProposalContextFactory<K, V> {
      ProposalContext<K, V> Create(ProposalLeaderPrepare<K> proposal);
   }

   public class ProposalContextFactoryImpl<K, V> : ProposalContextFactory<K, V> {
      private readonly ProposalPhaseFactory<K, V> proposalPhaseFactory;

      public ProposalContextFactoryImpl(ProposalPhaseFactory<K, V> proposalPhaseFactory) {
         this.proposalPhaseFactory = proposalPhaseFactory;
      }

      public ProposalContext<K, V> Create(ProposalLeaderPrepare<K> proposal) {
         var context = new ProposalContextImpl<K, V>(proposal);
         context.Initialize(proposalPhaseFactory.Initial(context));
         return context;
      }
   }
}
