using System;
using System.Collections.Generic;
using System.Linq;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;

namespace Dargon.Hydar.Clustering.Phases {
   public class ElectionPhase : PhaseBase {
      private readonly object synchronization = new object();
      private Guid lastSelectedCandidate = Guid.Empty;
      private Guid selectedCandidate;
      private Guid bestAcknowledgingLeader;
      private List<Guid> currentRoundVotes = new List<Guid>();
      private ItzWarty.Collections.ISet<Guid> allParticipants = new ItzWarty.Collections.HashSet<Guid>();
      private int electionSecurity = 0;

      public ElectionPhase(AuditEventBus auditEventBus, HydarContext context, ManageableClusterContext manageableClusterContext, NodePhaseFactory phaseFactory) : base(auditEventBus, context, manageableClusterContext, phaseFactory) {}

      public override void Initialize() {
         base.Initialize();

         selectedCandidate = node.Identifier;
         bestAcknowledgingLeader = node.Identifier;

         RegisterHandler<ElectionVote>(HandleElectionVote);
         RegisterHandler<ElectionAcknowledgement>(HandleElectionAcknowledgement);
         RegisterHandler<LeaderHeartBeat>(HandleLeaderHeartBeat);
         RegisterNullHandler<MemberHeartBeat>();
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
            var isNonLeader = selectedCandidate != node.Identifier;
            var isStableVote = lastSelectedCandidate == selectedCandidate;
            var isUnanimousVote = currentRoundVoteSet.Count == 1;
            var isAcknowledged = selectedCandidate == bestAcknowledgingLeader;
            if (isNonLeader && isStableVote && isUnanimousVote && isAcknowledged) {
               // Log("Silent");
               // silence - we're voting for another and have advertised our opinion
            } else {
               // Log("{0} : {1}".F(selectedCandidate.ToString("n").Substring(0, 8), isNonLeader + " " + isStableVote + " " + isUnanimousVote + " " + isAcknowledged));
               // Log("Voting for " + selectedCandidate.ToString("n").Substring(0, 8));
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
         lock (synchronization) {
            allParticipants.Add(header.SenderGuid);
            if (selectedCandidate.CompareTo(vote.CandidateGuid) < 0) {
               selectedCandidate = vote.CandidateGuid;
            }
            if (vote.CandidateGuid == node.Identifier) {
               Log("Acknowledging " + header.SenderGuid.ToString("n").Substring(0, 8));
               Send(new ElectionAcknowledgement(header.SenderGuid));
            } else {
               electionSecurity = 0;
            }
            currentRoundVotes.Add(vote.CandidateGuid);
         }
      }

      private void HandleElectionAcknowledgement(IRemoteIdentity remoteIdentity, HydarMessageHeader header, ElectionAcknowledgement acknowledgement) {
         lock (synchronization) {
            if (acknowledgement.AcknowledgedVoter == node.Identifier) {
               if (bestAcknowledgingLeader.CompareTo(header.SenderGuid) < 0) {
                  bestAcknowledgingLeader = header.SenderGuid;
               }
            } else {
               electionSecurity = 0;
            }
         }
      }

      private void HandleLeaderHeartBeat(IRemoteIdentity leader, HydarMessageHeader header, LeaderHeartBeat payload) {
         if (payload.ParticipantIds.Contains(node.Identifier)) {
            clusterContext.EnterEpoch(payload.EpochId, header.SenderGuid, payload.ParticipantIds);
            clusterContext.Transition(phaseFactory.CreateMemberPhase());
         } else {
            clusterContext.Transition(phaseFactory.CreateDroppedPhase());
         }
      }
   }
}
