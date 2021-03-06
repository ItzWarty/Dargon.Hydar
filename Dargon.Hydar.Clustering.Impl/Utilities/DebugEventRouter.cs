﻿using System;
using Dargon.Audits;
using Dargon.Hydar.Clustering.Phases;
using Dargon.Hydar.Networking;
using Dargon.Hydar.Utilities;
using ItzWarty;

namespace Dargon.Hydar.Clustering.Utilities {
   public interface DebugEventRouter {
      void ElectionCandidatePhase_RejoinEpoch(Guid epochId);
      void FollowerPhase_EndOfEpoch(Guid epochId);
      void FollowerPhase_LeaderMissedHeartBeats(int heartbeats);
      void LeaderPhase_SendHeartBeat(int participantCount);
      void ClusteringPhaseManager_Transition(IPhase previousPhase, IPhase nextPhase);
      void RootMessageDispatcher_DispatchFailed(Type payloadType);
   }

   public class DebugEventRouterImpl : DebugEventRouter {
      private readonly HydarIdentity identity;
      private readonly AuditEventBus auditEventBus;

      public DebugEventRouterImpl(HydarIdentity identity, AuditEventBus auditEventBus) {
         this.identity = identity;
         this.auditEventBus = auditEventBus;
      }

      public void ElectionCandidatePhase_RejoinEpoch(Guid epochId) {
         Log("Rejoining epoch {0}".F(GetGuidShortHand(epochId)));
      }

      public void FollowerPhase_EndOfEpoch(Guid epochId) {
         Log("End of epoch {0}".F(GetGuidShortHand(epochId)));
      }

      public void FollowerPhase_LeaderMissedHeartBeats(int heartbeats) {
         Log("Leader missed {0} heartbeats".F(heartbeats));
      }

      public void LeaderPhase_SendHeartBeat(int participantCount) {
         Log("Sending heartbeat to {0} participants".F(participantCount));
      }

      public void ClusteringPhaseManager_Transition(IPhase previousPhase, IPhase nextPhase) {
         Log(GetTypeString(previousPhase) + " => " + GetTypeString(nextPhase));
      }

      public void RootMessageDispatcher_DispatchFailed(Type payloadType) {
         auditEventBus.Error(HydarConstants.kAuditEventBusErrorKey, "Unknown Payload Type", "Payload Type: {1}".F(payloadType.FullName));
      }

      private void Log(string message) {
         string output = identity.NodeId.ToString("n").Substring(0, 8) + ": " + message;
         Console.WriteLine(output);
      }

      private string GetTypeString(object obj) {
         if (obj == null) {
            return "null";
         } else {
            return obj.GetType().Name;
         }
      }

      private string GetGuidShortHand(Guid epochId) {
         return epochId.ToString("n").Substring(0, 8);
      }
   }
}
