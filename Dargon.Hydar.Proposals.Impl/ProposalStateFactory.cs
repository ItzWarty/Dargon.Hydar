using System;
using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Phases;
using ItzWarty;

namespace Dargon.Hydar.Proposals {
   public interface ProposalStateFactory<TSubject> {
      ProposalState<TSubject> Create(Guid proposalId, Proposal<TSubject> proposal);
   }

   public class ProposalStateFactoryImpl<TSubject> : ProposalStateFactory<TSubject> {
      private readonly Guid cacheId;
      private readonly SubjectStateManager<TSubject> subjectStateManager;
      private readonly ProposalMessageSender<TSubject> proposalMessageSender;

      public ProposalStateFactoryImpl(Guid cacheId, SubjectStateManager<TSubject> subjectStateManager, ProposalMessageSender<TSubject> proposalMessageSender) {
         this.cacheId = cacheId;
         this.subjectStateManager = subjectStateManager;
         this.proposalMessageSender = proposalMessageSender;
      }

      public ProposalState<TSubject> Create(Guid proposalId, Proposal<TSubject> proposal) {
         var subjectState = subjectStateManager.GetOrCreate(proposal.Subject);
         var proposalState = new ProposalStateImpl<TSubject>(proposalId, proposal);
         var proposalPhaseFactory = new ProposalPhaseFactoryImpl<TSubject>(proposalState, subjectState, proposalMessageSender);
         proposalState.Initialize(proposalPhaseFactory.FollowerInitialPhase());
         return proposalState;
      }
   }
}
