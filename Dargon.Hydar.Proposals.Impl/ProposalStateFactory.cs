using System;
using Dargon.Hydar.Proposals.Messages;
using Dargon.Hydar.Proposals.Phases;

namespace Dargon.Hydar.Proposals {
   public interface ProposalStateFactory<TSubject> {
      ProposalState<TSubject> Create(Guid proposalId, Proposal<TSubject> proposal);
   }

   public class ProposalStateFactoryImpl<TSubject> : ProposalStateFactory<TSubject> {
      private readonly ProposalPhaseFactory<TSubject> proposalPhaseFactory;

      public ProposalStateFactoryImpl(ProposalPhaseFactory<TSubject> proposalPhaseFactory) {
         this.proposalPhaseFactory = proposalPhaseFactory;
      }

      public ProposalState<TSubject> Create(Guid proposalId, Proposal<TSubject> proposal) {
         var state = new ProposalStateImpl<TSubject>(proposalId, proposal);
         state.Initialize(proposalPhaseFactory.Initial());
         return state;
      }
   }
}
