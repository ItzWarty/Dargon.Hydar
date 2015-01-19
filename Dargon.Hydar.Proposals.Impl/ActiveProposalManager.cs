namespace Dargon.Hydar.Proposals {
   public interface ActiveProposalManager<K, V> {
      bool TryBully(K key, ProposalContext<K, V> candidate);
      bool TryDeactivate(ProposalContext<K, V> proposalContext);
   }
}
