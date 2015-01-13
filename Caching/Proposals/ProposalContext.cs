using System;
using Dargon.Hydar.Caching.Data.Operations;
using Dargon.Hydar.Caching.Proposals.Messages;
using Dargon.Hydar.Caching.Proposals.Phases;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Caching.Proposals {
   public interface ProposalContext<K, V> {
      ProposalLeaderPrepare<K> Proposal { get; }

      void Transition(IProposalPhase<K, V> newPhase);
      void Step();
      void Process(InboundEnvelope envelope);
      bool TryBullyWith(ProposalContext<K, V> candidate);
   }

   public class ProposalContextImpl<K, V> : ProposalContext<K, V> {
      private readonly object synchronization = new object();
      private readonly ProposalLeaderPrepare<K> proposal;
      private IProposalPhase<K, V> phase;

      public ProposalContextImpl(ProposalLeaderPrepare<K> proposal) {
         this.proposal = proposal;
      }

      public ProposalLeaderPrepare<K> Proposal { get { return proposal; } }

      public void Initialize(IProposalPhase<K, V> initialPhase) {
         Transition(initialPhase);
      }

      public void Transition(IProposalPhase<K, V> newPhase) {
         lock (synchronization) {
            phase = newPhase;
            phase.HandleEnter();
         }
      }

      public void Step() {
         lock (synchronization) {
            phase.Step();
         }
      }

      public void Process(InboundEnvelope envelope) {
         lock (synchronization) {
            phase.Process(envelope);
         }
      }

      public bool TryBullyWith(ProposalContext<K, V> candidate) {
         lock (synchronization) {
            return phase.TryBullyWith(candidate);
         }
      }
   }
}
