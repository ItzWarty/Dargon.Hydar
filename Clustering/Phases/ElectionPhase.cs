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
      private Guid selectedCandidateLastEpochId;
      private List<Guid> currentRoundVotes = new List<Guid>();
      private ItzWarty.Collections.ISet<Guid> allParticipants = new ItzWarty.Collections.HashSet<Guid>();
      private ItzWarty.Collections.ISet<Guid> allAcknowledgers = new ItzWarty.Collections.HashSet<Guid>();
      private int electionSecurity = 0;

      public ElectionPhase(
         AuditEventBus auditEventBus, 
         HydarContext context, 
         ManageableClusterContext manageableClusterContext, 
         NodePhaseFactory phaseFactory, 
         Guid selectedCandidateLastEpochId
      ) : base(auditEventBus, context, manageableClusterContext, phaseFactory) {
         // can't use value from clustering context - otherwise, dropped nodes
         // would advertise stale epochs.
         this.selectedCandidateLastEpochId = selectedCandidateLastEpochId;
      }

      public override void Initialize() {
         base.Initialize();

         selectedCandidate = node.Identifier;
         allAcknowledgers.Add(node.Identifier);

         RegisterHandler<ElectionVote>(HandleElectionVote);
         RegisterHandler<ElectionAcknowledgement>(HandleElectionAcknowledgement);
         RegisterHandler<LeaderHeartBeat>(HandleLeaderHeartBeat);
         RegisterHandler<MemberHeartBeat>(HandleMemberHeartBeat);
      }

      public override void Enter() {
         base.Enter();

         lock (synchronization) {
            allParticipants.Add(node.Identifier);
            Send(new ElectionVote(selectedCandidate, selectedCandidateLastEpochId));
         }
      }

      public override void Tick() {
         lock (synchronization) {
            var currentRoundVoteSet = new ItzWarty.Collections.HashSet<Guid>(currentRoundVotes);
            var isNonLeader = selectedCandidate != node.Identifier;
            var isStableVote = lastSelectedCandidate == selectedCandidate;
            var isUnanimousVote = currentRoundVoteSet.Count == 1;
            var isAcknowledged = allAcknowledgers.Contains(selectedCandidate);
            if (isNonLeader && isStableVote && isUnanimousVote && isAcknowledged) {
               // Log("Silent");
               // silence - we're voting for another and have advertised our opinion
            } else {
               // Log("{0} : {1}".F(selectedCandidate.ToString("n").Substring(0, 8), isNonLeader + " " + isStableVote + " " + isUnanimousVote + " " + isAcknowledged));
               // Log("Voting for " + selectedCandidate.ToString("n").Substring(0, 8));
               Send(new ElectionVote(selectedCandidate, selectedCandidateLastEpochId));

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
            // A new candidate (with previous epoch zero) should give way to older cluster members.
            var isMaturerCandidate = selectedCandidateLastEpochId == Guid.Empty && vote.LastEpochId != Guid.Empty;
            
            // Between two candidates of the same previous guid, we pick the one with the highest id.
            var isGreaterCandidate = selectedCandidateLastEpochId == vote.LastEpochId && selectedCandidate.CompareTo(vote.CandidateGuid) < 0;

            if (isMaturerCandidate || isGreaterCandidate) {
               selectedCandidate = vote.CandidateGuid;
               selectedCandidateLastEpochId = vote.LastEpochId;
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
               Log("Acknowledged by " + header.SenderGuid.ToString("n").Substring(0, 8));
               allAcknowledgers.Add(header.SenderGuid);
            } else {
               electionSecurity = 0;
            }
         }
      }

      private void HandleLeaderHeartBeat(IRemoteIdentity leader, HydarMessageHeader header, LeaderHeartBeat payload) {
         if (DateTime.Now >= payload.Interval.End) {
            return; // throw away stale message
         }
         if (payload.ParticipantIds.Contains(node.Identifier)) {
            clusterContext.EnterEpoch(payload.EpochId, payload.Interval, header.SenderGuid, payload.ParticipantIds, payload.LastEpochId);
            clusterContext.Transition(phaseFactory.CreateMemberPhase());
         } else {
            clusterContext.Transition(phaseFactory.CreateDroppedPhase(payload.Interval.End));
         }
      }

      private void HandleMemberHeartBeat(IRemoteIdentity member, HydarMessageHeader header, MemberHeartBeat payload) {
         lock (synchronization) {
            electionSecurity = 0;
         }
      }
   }
}
