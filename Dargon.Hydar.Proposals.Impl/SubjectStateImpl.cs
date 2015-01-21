using System;
using ItzWarty.Collections;

namespace Dargon.Hydar.Proposals {
   public class SubjectStateImpl<TSubject> : SubjectState<TSubject> {
      private readonly object activeProposalSynchronization = new object();
      private readonly IConcurrentQueue<Proposal<TSubject>> localProposals = new ConcurrentQueue<Proposal<TSubject>>();
      private readonly AtomicExecutionContext<TSubject> atomicExecutionContext;
      private readonly ProposalStateManager<TSubject> proposalStateManager;
      private readonly TSubject subject;
      private ProposalState<TSubject> activeProposalState = null;

      public SubjectStateImpl(AtomicExecutionContext<TSubject> atomicExecutionContext, TSubject subject, ProposalStateManager<TSubject> proposalStateManager) {
         this.atomicExecutionContext = atomicExecutionContext;
         this.subject = subject;
         this.proposalStateManager = proposalStateManager;
      }

      public void EnqueueProposal(Proposal<TSubject> proposal) {
         localProposals.Enqueue(proposal);

         lock (activeProposalSynchronization) {
            if (activeProposalState == null) {
               var proposalState = proposalStateManager.CreateLeaderState(proposal);
               TryBullyCurrentProposal(proposalState);
            }
         }
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

               Proposal<TSubject> nextProposal;
               if (localProposals.TryDequeue(out nextProposal)) {
                  var nextProposalState = proposalStateManager.CreateLeaderState(nextProposal);
                  TryBullyCurrentProposal(nextProposalState);
               }
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