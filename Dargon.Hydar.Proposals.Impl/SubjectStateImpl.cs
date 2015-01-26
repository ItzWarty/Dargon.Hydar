using System;
using System.Diagnostics;
using ItzWarty.Collections;

namespace Dargon.Hydar.Proposals {
   public class SubjectStateImpl<TSubject> : SubjectState<TSubject> {
      private readonly object activeProposalSynchronization = new object();
      private readonly IConcurrentQueue<Proposal<TSubject>> localProposalQueue = new ConcurrentQueue<Proposal<TSubject>>();
      private readonly AtomicExecutionContext<TSubject> atomicExecutionContext;
      private readonly ProposalStateManager<TSubject> proposalStateManager;
      private readonly TSubject subject;
      private ProposalState<TSubject> activeProposalState = null;

      public SubjectStateImpl(AtomicExecutionContext<TSubject> atomicExecutionContext, TSubject subject, ProposalStateManager<TSubject> proposalStateManager) {
         this.atomicExecutionContext = atomicExecutionContext;
         this.subject = subject;
         this.proposalStateManager = proposalStateManager;
      }

      public void Signal() {
         lock (activeProposalSynchronization) {
            if (activeProposalState == null) {
               Proposal<TSubject> proposal;
               if (localProposalQueue.TryDequeue(out proposal)) {
                  var proposalState = proposalStateManager.CreateLeaderState(proposal);
                  var proposalBullySuccessful = TryBullyCurrentProposal(proposalState);
                  Trace.Assert(proposalBullySuccessful);
               }
            }
         }
      }

      public void EnqueueProposal(Proposal<TSubject> proposal) {
         localProposalQueue.Enqueue(proposal);

         Signal();
      }

      public void ExecuteProposal(Proposal<TSubject> proposal) {
         atomicExecutionContext.Execute(subject, proposal);
      }

      public void HandleProposalTermination(ProposalState<TSubject> proposalState) {
         lock (activeProposalSynchronization) {
            if (activeProposalState != proposalState) {
               throw new InvalidOperationException();
            } else {
               activeProposalState = null;

               Signal();
            }
         }
      }

      public bool TryBullyCurrentProposal(ProposalState<TSubject> suggestedProposalState) {
         if (suggestedProposalState == null) {
            throw new ArgumentNullException("suggestedProposalState");
         }

         lock (activeProposalSynchronization) {
            if (activeProposalState == null || (activeProposalState.ProposalId.CompareTo(suggestedProposalState.ProposalId) < 0 && activeProposalState.TryCancel())) {
               activeProposalState = suggestedProposalState;
               return true;
            }
            return false;
         }
      }
   }
}