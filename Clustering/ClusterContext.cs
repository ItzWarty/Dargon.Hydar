using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty.Collections;
using System;
using System.Collections.Generic;
using Dargon.Hydar.Clustering.Phases;

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
      void EnterEpoch(Guid epochId, Guid leaderGuid, IReadOnlySet<Guid> participantGuids);
      void HandlePeerHeartBeat(Guid peerGuid);
   }
}
