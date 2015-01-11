using System;
using Dargon.Hydar.Caching.Data.Operations;
using Dargon.Hydar.Caching.Proposals.Messages;
using Dargon.Hydar.Caching.Proposals.Phases;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Caching.Proposals {
   public interface ProposalContext<K, V> {
      ProposalLeaderPrepare<K> Proposal { get; }

      void Transition(IProposalPhase newPhase);
      void Step();
      void Process(InboundEnvelope envelope);
   }

   public class ProposalContextImpl<K, V> : ProposalContext<K, V> {
      private readonly object synchronization = new object();
      private readonly ProposalLeaderPrepare<K> proposal;
      private IProposalPhase phase;

      public ProposalContextImpl(ProposalLeaderPrepare<K> proposal) {
         this.proposal = proposal;
      }

      public ProposalLeaderPrepare<K> Proposal { get { return proposal; } }

      public void Initialize(IProposalPhase initialPhase) {
         Transition(initialPhase);
      }

      public void Transition(IProposalPhase newPhase) {
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
   }
}
