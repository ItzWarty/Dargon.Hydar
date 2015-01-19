using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Proposals {
   public class ActiveProposalManagerImpl<K, V> : ActiveProposalManager<K, V> {
      private readonly IConcurrentDictionary<K, SubjectState<>> activeProposalContextsByEntryKey = new ConcurrentDictionary<K, SubjectState<>>();

      public bool TryBully(K key, SubjectState<> candidate) {
         bool successful = true;
         activeProposalContextsByEntryKey.AddOrUpdate(
            key,
            candidate,
            (key_, previous) => TryBullyCompareHelper(previous, candidate, out successful)
            );
         return successful;
      }

      internal SubjectState<> TryBullyCompareHelper(SubjectState<> previous, SubjectState<> candidate, out bool bullySuccessful) {
         if (previous.TryBullyWith(candidate)) {
            bullySuccessful = true;
            return candidate;
         } else {
            bullySuccessful = false;
            return previous;
         }
      }

      public bool TryDeactivate(SubjectState<> subjectState) {
         return activeProposalContextsByEntryKey.TryRemove(subjectState.AtomicProposal.EntryKey, subjectState);
      }
   }
}