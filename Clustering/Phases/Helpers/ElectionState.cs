using System;
using Dargon.Hydar.Clustering.Messages.Helpers;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering.Phases.Helpers {
   public interface ElectionState {
      IReadOnlySet<Guid> Participants { get; }

      void AddParticipant(Guid guid);
      void AddAcknowledger(Guid guid);

      ElectionCandidate SelectedCandidate { get; set; }
      bool IsAcknowledgedBySelectedCandidate();
      ConsiderationResult ConsiderCandidate(ElectionCandidate candidate);
   }

   public enum ConsiderationResult {
      SuggestionEquivalent,
      SuggestionWorse,
      SuggestionBetter
   }

   public class ElectionStateImpl : ElectionState {
      private readonly ISet<Guid> participants = new HashSet<Guid>();
      private readonly ISet<Guid> acknowledgers = new HashSet<Guid>();
      private ElectionCandidate selectedCandidate;

      public IReadOnlySet<Guid> Participants { get { return participants; } }

      public void AddParticipant(Guid guid) {
         participants.Add(guid);
      }

      public void AddAcknowledger(Guid guid) {
         acknowledgers.Add(guid);
      }

      public ElectionCandidate SelectedCandidate { get { return selectedCandidate; } set { selectedCandidate = value; } }

      public bool IsAcknowledgedBySelectedCandidate() {
         return acknowledgers.Contains(selectedCandidate.Id);
      }

      public ConsiderationResult ConsiderCandidate(ElectionCandidate candidate) {
         if (selectedCandidate == null) {
            selectedCandidate = candidate;
            return ConsiderationResult.SuggestionBetter;
         } else {
            // A new candidate (with previous epoch zero) should give way to older cluster members.
            var isMaturerCandidate = selectedCandidate.LastEpochId == Guid.Empty && candidate.LastEpochId != Guid.Empty;

            // Between two candidates of the same previous guid, we pick the one with the highest id.
            var isGreaterCandidate = selectedCandidate.LastEpochId == candidate.LastEpochId && selectedCandidate.Id.CompareTo(candidate.Id) < 0;

            if (isMaturerCandidate || isGreaterCandidate) {
               selectedCandidate = candidate;
               return ConsiderationResult.SuggestionBetter;
            } else if (selectedCandidate.Id == candidate.Id) {
               return ConsiderationResult.SuggestionEquivalent;
            } else {
               return ConsiderationResult.SuggestionWorse;
            }
         }
      }
   }
}
