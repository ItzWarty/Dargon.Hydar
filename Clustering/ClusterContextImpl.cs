using System;
using System.Collections.Generic;
using Dargon.Hydar.Clustering.Peering;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Clustering {
   public class ClusterContextImpl : ManageableClusterContext {
      public event NewEpochHandler NewEpoch;
      private readonly HydarContext context;
      private readonly ClusteringConfiguration configuration;
      private readonly NodePhaseFactory phaseFactory;
      private readonly Dictionary<Guid, EpochDescriptor> epochsById = new Dictionary<Guid, EpochDescriptor>();
      private readonly Dictionary<Guid, DateTime> heartBeatTimesByNodeId = new Dictionary<Guid, DateTime>();
      private readonly object synchronization = new object();
      private EpochDescriptor currentEpoch = new EpochDescriptorImpl(Guid.Empty, Guid.Empty, new ItzWarty.Collections.HashSet<Guid>());
      private IPeeringPhase currentPeeringPhase;

      public ClusterContextImpl(HydarContext context, ClusteringConfiguration configuration, NodePhaseFactory phaseFactory) {
         this.context = context;
         this.configuration = configuration;
         this.phaseFactory = phaseFactory;
      }

      public void Initialize() {
         currentPeeringPhase = phaseFactory.CreateInitializationPhase();
         Transition(currentPeeringPhase);
      }

      #region ClusterContext Implementation
      public EpochDescriptor GetCurrentEpoch() {
         return currentEpoch;
      }

      public IReadOnlyDictionary<Guid, PeerStatus> GetPeerStatuses() {
         throw new NotImplementedException();
      }

      public IPeeringPhase __DebugCurrentPhase { get { return currentPeeringPhase; } }
      #endregion

      #region ManageableClusterContext Implementation
      public void Tick() {
         lock (synchronization) {
            currentPeeringPhase.Tick();
         }
      }

      public void Transition(IPeeringPhase peeringPhase) {
         lock (synchronization) {
            peeringPhase.ThrowIfNull("phase");
            Log("=> " + peeringPhase);
            currentPeeringPhase = peeringPhase;
            currentPeeringPhase.Enter();
         }
      }

      public bool Process(IRemoteIdentity senderIdentity, HydarMessage message) {
         lock (synchronization) {
            return currentPeeringPhase.Process(senderIdentity, message);
         }
      }

      public void EnterEpoch(Guid epochId, Guid leaderGuid, IReadOnlySet<Guid> participantGuids) {
         var epoch = new EpochDescriptorImpl(epochId, leaderGuid, participantGuids);
         currentEpoch = epoch;
      }
      #endregion

      public void Log(string x) {
         context.Log(context.Node.Identifier.ToString("n").Substring(0, 8) + " " + x);
      }
   }
}