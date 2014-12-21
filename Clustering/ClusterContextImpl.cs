using System;
using System.Collections.Generic;
using Dargon.Hydar.Clustering.Phases;
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
      private IPhase currentPhase;

      public ClusterContextImpl(HydarContext context, ClusteringConfiguration configuration, NodePhaseFactory phaseFactory) {
         this.context = context;
         this.configuration = configuration;
         this.phaseFactory = phaseFactory;
      }

      public void Initialize() {
         currentPhase = phaseFactory.CreateInitializationPhase();
         Transition(currentPhase);
      }

      #region ClusterContext Implementation
      public EpochDescriptor GetCurrentEpoch() {
         return currentEpoch;
      }

      public IReadOnlyDictionary<Guid, PeerStatus> GetPeerStatuses() {
         throw new NotImplementedException();
      }

      public IPhase __DebugCurrentPhase { get { return currentPhase; } }
      #endregion

      #region ManageableClusterContext Implementation
      public void Tick() {
         lock (synchronization) {
            currentPhase.Tick();
         }
      }

      public void Transition(IPhase phase) {
         lock (synchronization) {
            phase.ThrowIfNull("phase");
            Log("=> " + phase);
            currentPhase = phase;
            currentPhase.Enter();
         }
      }

      public bool Process(IRemoteIdentity senderIdentity, HydarMessage message) {
         lock (synchronization) {
            return currentPhase.Process(senderIdentity, message);
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