using System;
using System.Collections.Generic;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Grid.Peering {
   public interface PeeringContext {
      EpochDescriptor GetCurrentEpoch();

      void SetPhase(IPeeringPhase peeringPhase);
      IPeeringPhase GetCurrentPhase();
      bool Process(IRemoteIdentity senderIdentity, HydarMessage message);

      void EnterEpoch(Guid guid, IReadOnlySet<Guid> participantGuids);
      IReadOnlySet<Guid> GetActivePeers();
      void HandlePeerHeartBeat(Guid peerGuid);
      void Tick();
   }

   public class PeeringContextImpl : PeeringContext {
      private readonly GridConfiguration configuration;
      private readonly NodePhaseFactory phaseFactory;
      private readonly HydarContext context;
      private readonly Dictionary<Guid, EpochDescriptor> epochsById = new Dictionary<Guid, EpochDescriptor>();
      private readonly Dictionary<Guid, DateTime> heartBeatTimesByNodeId = new Dictionary<Guid, DateTime>();
      private readonly object synchronization = new object();
      private EpochDescriptor currentEpoch = new EpochDescriptorImpl(Guid.Empty, new ItzWarty.Collections.HashSet<Guid>());
      private IPeeringPhase currentPeeringPhase;

      public PeeringContextImpl(GridConfiguration configuration, NodePhaseFactory phaseFactory, HydarContext context) {
         this.configuration = configuration;
         this.phaseFactory = phaseFactory;
         this.context = context;
      }

      public EpochDescriptor GetCurrentEpoch() {
         return currentEpoch;
      }

      public void EnterEpoch(Guid guid, IReadOnlySet<Guid> participantGuids) {
         var epoch = new EpochDescriptorImpl(guid, participantGuids);
         currentEpoch = epoch;
      }

      public IReadOnlySet<Guid> GetActivePeers() {
         throw new NotImplementedException();
      }

      public void Initialize() {
         currentPeeringPhase = phaseFactory.CreateInitializationPhase();
         SetPhase(currentPeeringPhase);
      }

      public void SetPhase(IPeeringPhase peeringPhase) {
         lock (synchronization) {
            peeringPhase.ThrowIfNull("phase");
            Log("=> " + peeringPhase);
            currentPeeringPhase = peeringPhase;
            currentPeeringPhase.Enter();
         }
      }

      public IPeeringPhase GetCurrentPhase() {
         return currentPeeringPhase;
      }

      public bool Process(IRemoteIdentity senderIdentity, HydarMessage message) {
         lock (synchronization) {
            return currentPeeringPhase.Process(senderIdentity, message);
         }
      }

      public void Tick() {
         lock (synchronization) {
            currentPeeringPhase.Tick();
         }
      }

      public void HandlePeerHeartBeat(Guid peerGuid) {
         throw new NotImplementedException();
      }

      public void Log(string x) {
         context.Log(context.Node.Identifier.ToString("n").Substring(0, 8) + " " + x);
      }
   }
}
