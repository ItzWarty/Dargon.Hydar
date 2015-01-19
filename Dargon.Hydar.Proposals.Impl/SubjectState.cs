using System;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Phases;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Proposals {
   public interface SubjectState<TSubject> {
      bool TryBullyCurrentProposal(ProposalState<TSubject> suggestedProposalState);
   }

   public class SubjectStateImpl<TSubject> : SubjectState<TSubject> {
      private readonly object activeProposalSynchronization = new object();
      private readonly IConcurrentQueue<Proposal<TSubject>> localProposals = new ConcurrentQueue<Proposal<TSubject>>();
      private readonly AtomicExecutionContext<TSubject> atomicExecutionContext;
      private readonly TSubject subject;
      private ProposalState<TSubject> activeProposalState = null;

      public SubjectStateImpl(AtomicExecutionContext<TSubject> atomicExecutionContext, TSubject subject) {
         this.atomicExecutionContext = atomicExecutionContext;
         this.subject = subject;
      }

      public void EnqueueProposal(Proposal<TSubject> proposal) {
         localProposals.Enqueue(proposal);
      }

      public void ExecuteProposal(Proposal<TSubject> proposal) {
         atomicExecutionContext.Execute(subject, proposal);
      }

      public bool TryBullyCurrentProposal(ProposalState<TSubject> suggestedProposalState) {
         if (suggestedProposalState == null) {
            throw new ArgumentNullException("suggestedProposalState");
         }

         lock (activeProposalSynchronization) {
            if (activeProposalState.ProposalId.CompareTo(suggestedProposalState.ProposalId) < 0) {
               if (activeProposalState.TryCancel()) {
                  activeProposalState = suggestedProposalState;
                  return true;
               }
            }
            return false;
         }
      }
   }
}
