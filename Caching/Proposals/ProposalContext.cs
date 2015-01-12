using System;
using Dargon.Hydar.Caching.Data.Operations;
using Dargon.Hydar.Caching.Proposals.Messages;
using Dargon.Hydar.Caching.Proposals.Phases;
using Dargon.Hydar.PortableObjects;
using ItzWarty.Collections;

namespace Dargon.Hydar.Caching.Proposals {
   public interface ProposalContext<K, V> {
      ProposalLeaderPrepare<K> Proposal { get; }

      void Transition(IProposalPhase<K, V> newPhase);
      void Step();
      void ProcessLeaderPrepare(InboundEnvelope envelope, IConcurrentDictionary<K, ProposalContext<K, V>> activeProposalContextsByEntryKey);
      void ProcessLeaderCommit(InboundEnvelope envelope);
      void ProcessLeaderCancel(InboundEnvelope envelope);
      void ProcessFollowerAccept(InboundEnvelope envelope);
      void ProcessFollowerReject(InboundEnvelope envelope);
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

      public void ProcessLeaderPrepare(InboundEnvelope envelope, IConcurrentDictionary<K, ProposalContext<K, V>> activeProposalContextsByEntryKey) {
         lock (synchronization) {
            phase.ProcessLeaderPrepare(envelope.Header, (ProposalLeaderPrepare<K>)envelope.Message, activeProposalContextsByEntryKey);
         }
      }

      public void ProcessLeaderCommit(InboundEnvelope envelope) {
         lock (synchronization) {
            phase.ProcessLeaderCommit(envelope.Header, (ProposalLeaderCommit)envelope.Message);
         }
      }

      public void ProcessLeaderCancel(InboundEnvelope envelope) {
         lock (synchronization) {
            phase.ProcessLeaderCancel(envelope.Header, (ProposalLeaderCancel)envelope.Message);
         }
      }

      public void ProcessFollowerAccept(InboundEnvelope envelope) {
         lock (synchronization) {
            phase.ProcessFollowerAccept(envelope.Header, (ProposalFollowerAccept)envelope.Message);
         }
      }

      public void ProcessFollowerReject(InboundEnvelope envelope) {
         lock (synchronization) {
            phase.ProcessFollowerReject(envelope.Header, (ProposalFollowerReject)envelope.Message);
         }
      }
   }
}
