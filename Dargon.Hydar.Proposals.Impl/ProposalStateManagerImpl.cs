using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Proposals.Messages;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Proposals {
   public interface ProposalStateManager<TSubject> {
      ProposalState<TSubject> CreateLeaderState(Proposal<TSubject> proposal);
      void HandleProposalPrepare(Guid proposalId, Proposal<TSubject> proposal);
      ProposalState<TSubject> GetProposalStateByIdOrNull(Guid proposalId);
   }

   public class ProposalStateManagerImpl<TSubject> : ProposalStateManager<TSubject> {
      private readonly ProposalStateFactory<TSubject> proposalStateFactory;
      private readonly IConcurrentDictionary<Guid, ProposalState<TSubject>> proposalStatesById = new ConcurrentDictionary<Guid, ProposalState<TSubject>>();

      public ProposalStateManagerImpl(ProposalStateFactory<TSubject> proposalStateFactory) {
         this.proposalStateFactory = proposalStateFactory;
      }

      public ProposalState<TSubject> CreateLeaderState(Proposal<TSubject> proposal) {
         return proposalStatesById.AddOrUpdate(
            Guid.NewGuid(),
            proposalId => proposalStateFactory.CreateLeader(proposalId, proposal),
            (proposalId, existing) => proposalStateFactory.CreateLeader(proposalId, proposal)
         );
      }

      public void HandleProposalPrepare(Guid proposalId, Proposal<TSubject> proposal) {
         var state = proposalStatesById.AddOrUpdate(
            proposalId,
            proposalId_ => proposalStateFactory.CreateFollower(proposalId, proposal),
            (proposalId_, existing) => existing
         );
         state.HandlePrepare();
      }

      public ProposalState<TSubject> GetProposalStateByIdOrNull(Guid proposalId) {
         return proposalStatesById.GetValueOrDefault(proposalId);
      }
   }
}
