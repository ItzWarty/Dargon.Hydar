using System;
using Dargon.Hydar.Clustering.Messages.Helpers;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Clustering.Messages {
   public interface ClusteringMessageFactory {
      ElectionVote ElectionVote(ElectionCandidate candidate);
      ElectionAcknowledgement ElectionAcknowledgement(Guid acknowledgedVoter);
      EpochLeaderHeartBeat EpochLeaderHeartBeat(DateTimeInterval epochInterval, EpochSummary currentEpochSummary, EpochSummary previousEpochSummary);
   }

   public class ClusteringMessageFactoryImpl : ClusteringMessageFactory {
      public ElectionVote ElectionVote(ElectionCandidate candidate) {
         return new ElectionVote(candidate);
      }

      public ElectionAcknowledgement ElectionAcknowledgement(Guid acknowledgedVoter) {
         return new ElectionAcknowledgement(acknowledgedVoter);
      }


      public EpochLeaderHeartBeat EpochLeaderHeartBeat(DateTimeInterval epochInterval, EpochSummary currentEpochSummary, EpochSummary previousEpochSummary) {
         return new EpochLeaderHeartBeat(epochInterval, currentEpochSummary, previousEpochSummary);
      }
   }
}
