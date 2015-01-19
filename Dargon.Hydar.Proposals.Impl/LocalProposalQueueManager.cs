using System;
using Dargon.Hydar.Caching.Data.Operations;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Proposals {
   public interface LocalProposalQueueManager<K, V> {
      void EnqueueOperation(K key, EntryOperation<K, V> operation);
   }

   public class LocalProposalQueueManagerImpl<K, V> : LocalProposalQueueManager<K, V> {
      private readonly IConcurrentDictionary<K, IPriorityQueue<EnqueuedProposal<K, V>>> proposalQueuesByKey = new ConcurrentDictionary<K, IPriorityQueue<EnqueuedProposal<K, V>>>();

      public void EnqueueOperation(K key, EntryOperation<K, V> operation) {
         var proposal = new EnqueuedProposal<K, V>(key, operation, Guid.NewGuid());
         proposalQueuesByKey.AddOrUpdate(
            key,
            add => new PriorityQueue<EnqueuedProposal<K, V>>().With(x => x.Add(proposal)),
            (key_, existing) => existing.With(existing_ => existing_.Add(proposal))
         );
      }

      private class EnqueuedProposal<K, V> : IComparable<EnqueuedProposal<K, V>>  {
         private readonly K key;
         private readonly EntryOperation<K, V> operation;
         private readonly Guid proposalId;

         public EnqueuedProposal(K key, EntryOperation<K, V> operation, Guid proposalId) {
            this.key = key;
            this.operation = operation;
            this.proposalId = proposalId;
         }

         public K Key { get { return key; } }
         public EntryOperation<K, V> Operation { get { return operation; } }
         public Guid ProposalId { get { return proposalId; } }

         public int CompareTo(EnqueuedProposal<K, V> other) {
            return proposalId.CompareTo(other.proposalId);
         }
      }
   }
}
