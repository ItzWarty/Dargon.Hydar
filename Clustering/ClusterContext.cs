using System;
using System.Collections.Generic;
using Dargon.Hydar.Clustering.Peering;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering {
   public delegate void NewEpochHandler(EpochDescriptor epochDescriptor);

   public interface ClusterContext {
      event NewEpochHandler NewEpoch;

      EpochDescriptor GetCurrentEpoch();
      IReadOnlyDictionary<Guid, PeerStatus> GetPeerStatuses();

      IPeeringPhase __DebugCurrentPhase { get; }
   }

   public interface ManageableClusterContext : ClusterContext {
      void Tick();
      void Transition(IPeeringPhase peeringPhase);

      bool Process(IRemoteIdentity senderIdentity, HydarMessage message);
      void EnterEpoch(Guid epochId, Guid leaderGuid, IReadOnlySet<Guid> participantGuids);
   }
}
