using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Proposals {
   public interface ActiveProposalRegistry<K, V> {
      bool TryBully(K key, ProposalContext<K, V> candidate);
      bool TryDeactivate(ProposalContext<K, V> proposalContext);
   }

   public class ActiveProposalRegistryImpl<K, V> : ActiveProposalRegistry<K, V> {
      private readonly IConcurrentDictionary<K, ProposalContext<K, V>> activeProposalContextsByEntryKey = new ConcurrentDictionary<K, ProposalContext<K, V>>();

      public bool TryBully(K key, ProposalContext<K, V> candidate) {
         bool successful = true;
         activeProposalContextsByEntryKey.AddOrUpdate(
            key,
            candidate,
            (key_, previous) => TryBullyCompareHelper(previous, candidate, out successful)
         );
         return successful;
      }

      internal ProposalContext<K, V> TryBullyCompareHelper(ProposalContext<K, V> previous, ProposalContext<K, V> candidate, out bool bullySuccessful) {
         if (previous.TryBullyWith(candidate)) {
            bullySuccessful = true;
            return candidate;
         } else {
            bullySuccessful = false;
            return previous;
         }
      }

      public bool TryDeactivate(ProposalContext<K, V> proposalContext) {
         return activeProposalContextsByEntryKey.TryRemove(proposalContext.Proposal.EntryKey, proposalContext);
      }
   }
}
