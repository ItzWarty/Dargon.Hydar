using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.Networking.PortableObjects;
using Dargon.Hydar.Utilities;
using System;
using Dargon.Hydar.Clustering.Utilities;

namespace Dargon.Hydar.Clustering.Phases {
   public interface ClusteringPhaseManager {
      void Tick();
      void Transition(IPhase phase);
      IPhase GetCurrentPhase();
   }

   public class ClusteringPhaseManagerImpl : ClusteringPhaseManager {
      private readonly HydarIdentity identity;
      private readonly PeriodicTicker ticker;
      private readonly DebugEventRouter debugEventRouter;
      private readonly InboundEnvelopeBus inboundEnvelopeBus;
      private readonly object synchronization = new object();
      private IPhase currentPhase;

      public ClusteringPhaseManagerImpl(HydarIdentity identity, PeriodicTicker ticker, DebugEventRouter debugEventRouter, InboundEnvelopeBus inboundEnvelopeBus) {
         this.identity = identity;
         this.ticker = ticker;
         this.debugEventRouter = debugEventRouter;
         this.inboundEnvelopeBus = inboundEnvelopeBus;
      }

      public void Initialize() {
         ticker.Tick += (s, e) => Tick();
         inboundEnvelopeBus.EventPosted += HandleInboundEnvelope;
      }

      public void Tick() {
         lock (synchronization) {
            currentPhase.Tick();
         }
      }

      private void HandleInboundEnvelope(EventBus<InboundEnvelope> sender, InboundEnvelope envelope) {
         lock (synchronization) {
            var recipient = envelope.Header.RecipientId;
            if (recipient == Guid.Empty || recipient == identity.NodeId) {
               currentPhase.Process(envelope);
            }
         }
      }

      public void Transition(IPhase phase) {
         lock (synchronization) {
            debugEventRouter.ClusteringPhaseManager_Transition(currentPhase, phase);
            currentPhase = phase;
            if (phase != null) {
               phase.Enter();
            }
         }
      }

      public IPhase GetCurrentPhase() {
         return currentPhase;
      }
   }
}