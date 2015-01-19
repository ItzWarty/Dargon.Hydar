using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Phases;

namespace Dargon.Hydar.Proposals {
   public interface ProposalState<TSubject> {
      Guid ProposalId { get; }
      Proposal<TSubject> Proposal { get; }

      void Transition(IProposalPhase<TSubject> phase);

      void HandlePrepare();
      void HandleCommit();
      void HandleCancel();
      void HandleAccept();
      void HandleReject();

      bool TryCancel();
   }

   public class ProposalStateImpl<TSubject> : ProposalState<TSubject> {
      private readonly object synchronization = new object();
      private readonly Guid proposalId;
      private readonly Proposal<TSubject> proposal;
      private IProposalPhase<TSubject> phase;

      public ProposalStateImpl(Guid proposalId, Proposal<TSubject> proposal) {
         this.proposalId = proposalId;
         this.proposal = proposal;
      }

      public Guid ProposalId { get { return proposalId; } }
      public Proposal<TSubject> Proposal { get { return proposal; } }

      public void Initialize(IProposalPhase<TSubject> initialPhase) {
         Transition(initialPhase);
      }

      public void Transition(IProposalPhase<TSubject> nextPhase) {
         lock (synchronization) {
            if (nextPhase == null) {
               throw new ArgumentNullException("nextPhase");
            }

            phase = nextPhase;
            phase.HandleEnter();
         }
      }

      public void HandlePrepare() {
         lock (synchronization) {
            phase.HandlePrepare();
         }
      }

      public void HandleCommit() {
         lock (synchronization) {
            phase.HandleCommit();
         }
      }

      public void HandleCancel() {
         lock (synchronization) {
            phase.HandleCancel();
         }
      }

      public void HandleAccept() {
         lock (synchronization) {
            phase.HandleAccept();
         }
      }

      public void HandleReject() {
         lock (synchronization) {
            phase.HandleReject();
         }
      }

      public bool TryCancel() {
         lock (synchronization) {
            return phase.TryCancel();
         }
      }
   }
}
