using System;
using System.Collections.Generic;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;

namespace Dargon.Hydar.Clustering.Peering {
   public class ElectionPeeringPhase : PeeringPhaseBase {
      private readonly object synchronization = new object();
      private Guid lastSelectedCandidate = Guid.Empty;
      private Guid selectedCandidate;
      private List<Guid> currentRoundVotes = new List<Guid>();
      private ItzWarty.Collections.ISet<Guid> allParticipants = new ItzWarty.Collections.HashSet<Guid>();
      private int electionSecurity = 0;

      public ElectionPeeringPhase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory) : base(auditEventBus, context, manageableClusterContext, phaseFactory) {}

      public override void Initialize() {
         base.Initialize();

         selectedCandidate = node.Identifier;

         RegisterHandler<ElectionVote>(HandleElectionVote);
         RegisterHandler<LeaderHeartBeat>(HandleLeaderHeartBeat);
      }

      public override void Enter() {
         base.Enter();

         lock (synchronization) {
            allParticipants.Add(node.Identifier);
            Send(new ElectionVote(selectedCandidate));
         }
      }

      public override void Tick() {
         lock (synchronization) {
            var currentRoundVoteSet = new ItzWarty.Collections.HashSet<Guid>(currentRoundVotes);
            if (selectedCandidate != node.Identifier && lastSelectedCandidate == selectedCandidate && currentRoundVoteSet.Count == 1) {
               Log("Silent");
               // silence - we're voting for another and have advertised our opinion
            } else {
               Log("Voting for " + selectedCandidate.ToString("n").Substring(0, 8));
               Send(new ElectionVote(selectedCandidate));

               if (currentRoundVotes.Count == 1 && selectedCandidate == node.Identifier) {
                  electionSecurity++;

                  if (electionSecurity > configuration.ElectionTicksToPromotion) {
                     clusterContext.Transition(phaseFactory.CreateLeaderPhase(allParticipants));
                  }
               } else {
                  electionSecurity = 0;
               }

               currentRoundVotes.Clear();
               currentRoundVotes.Add(selectedCandidate);
            }
            lastSelectedCandidate = selectedCandidate;
         }
      }

      private void HandleElectionVote(IRemoteIdentity voter, HydarMessageHeader header, ElectionVote vote) {
         allParticipants.Add(header.SenderGuid);
         if (selectedCandidate.CompareTo(vote.CandidateGuid) < 0) {
            selectedCandidate = vote.CandidateGuid;
         }
         currentRoundVotes.Add(vote.CandidateGuid);
      }

      private void HandleLeaderHeartBeat(IRemoteIdentity leader, HydarMessageHeader header, LeaderHeartBeat payload) {
         clusterContext.EnterEpoch(payload.EpochId, header.SenderGuid, payload.ParticipantIds);
         clusterContext.Transition(phaseFactory.CreateMemberPhase());
      }
   }
}
