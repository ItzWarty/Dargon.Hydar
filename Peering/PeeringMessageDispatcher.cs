using System;
using System.Collections.Generic;
using Dargon.Hydar.Networking;
using Dargon.Hydar.Peering.Messages;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;
using ItzWarty.Collections;

namespace Dargon.Hydar.Peering {
   public interface PeeringMessageDispatcher {
   }

   public class PeeringMessageDispatcherImpl : EnvelopeProcessorBase<InboundEnvelope, Action<InboundEnvelope>>, PeeringMessageDispatcher {
      private readonly InboundEnvelopeBus inboundEnvelopeBus;
      private readonly ManageablePeeringState manageablePeeringState;

      public PeeringMessageDispatcherImpl(InboundEnvelopeBus inboundEnvelopeBus, ManageablePeeringState manageablePeeringState) {
         this.inboundEnvelopeBus = inboundEnvelopeBus;
         this.manageablePeeringState = manageablePeeringState;
      }

      public void Initialize() {
         inboundEnvelopeBus.EventPosted += (s, e) => Process(e);

         RegisterHandler<PeeringAnnounce>(HandlePeeringAnnounce);
      }

      private void HandlePeeringAnnounce(InboundEnvelopeHeader header, PeeringAnnounce peeringAnnounce) {
         manageablePeeringState.HandlePeeringAnnounce(header.SenderId, header.SenderAddress, peeringAnnounce);
      }

      private void RegisterHandler<TMessage>(Action<InboundEnvelopeHeader, TMessage> handler) {
         RegisterHandler<TMessage>((envelope) => handler(envelope.Header, (TMessage)envelope.Message));
      }

      protected override void Invoke(Action<InboundEnvelope> handler, InboundEnvelope envelope) {
         handler(envelope);
      }
   }
}
