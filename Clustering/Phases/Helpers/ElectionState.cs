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

      public IReadOnlySet<Guid> Participants { get { return participants; } }

      public void AddParticipant(Guid guid) {
         participants.Add(guid);
      }

      public void AddAcknowledger(Guid guid) {
         acknowledgers.Add(guid);
      }

      public ElectionCandidate SelectedCandidate { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }

      public bool IsAcknowledgedBySelectedCandidate() {
         throw new NotImplementedException();
      }

      public ConsiderationResult ConsiderCandidate(ElectionCandidate candidate) {
         throw new NotImplementedException();
      }
   }
}
