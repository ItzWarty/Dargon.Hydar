using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Caching.Data.Operations;
using ItzWarty.Collections;

namespace Dargon.Hydar.Caching.Proposals {
   public interface ProposalQueueManager<K, V> {
      void EnqueueOperation(K key, EntryOperation<K, V> operation);
   }

   public class ProposalQueueManagerImpl<K, V> : ProposalQueueManager<K, V> {
      private readonly object synchronization = new object();
      private readonly IPriorityQueue<EnqueuedProposal<K, V>> queue = new PriorityQueue<EnqueuedProposal<K, V>>();

      public void EnqueueOperation(K key, EntryOperation<K, V> operation) {
         lock (synchronization) {
            queue.Add(new EnqueuedProposal<K, V>(key, operation, Guid.NewGuid()));
         }
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
