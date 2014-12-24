using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty.Collections;
using System;
using System.Collections.Generic;
using Dargon.Hydar.Clustering.Phases;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Clustering {
   public delegate void NewEpochHandler(EpochDescriptor epochDescriptor);

   public interface ClusterContext {
      event NewEpochHandler NewEpoch;

      EpochDescriptor GetCurrentEpoch();
      IReadOnlyDictionary<Guid, PeerStatus> GetPeerStatuses();

      IPhase __DebugCurrentPhase { get; }
   }

   public interface ManageableClusterContext : ClusterContext {
      void Tick();
      void Transition(IPhase phase);

      bool Process(IRemoteIdentity senderIdentity, HydarMessage message);
      void EnterEpoch(Guid epochId, DateTimeInterval epochTimeInterval, Guid leaderGuid, IReadOnlySet<Guid> participantGuids, Guid previousEpochId);
      void HandlePeerHeartBeat(Guid peerGuid);
   }
}
