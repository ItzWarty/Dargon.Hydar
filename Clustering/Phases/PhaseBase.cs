using System;
using Dargon.Audits;
using Dargon.Hydar.Networking;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Clustering.Phases {
   public abstract class PhaseBase : EnvelopeProcessorBase<InboundEnvelope, Action<InboundEnvelope>>, IPhase {
      public virtual void Initialize() { }
      public virtual void Enter() { }
      public abstract void Tick();

      protected void RegisterHandler<TMessage>(Action<InboundEnvelopeHeader, TMessage> messageHandler) {
         base.RegisterHandler<TMessage>((envelope) => messageHandler(envelope.Header, (TMessage)GetMessage(envelope)));
      }

      protected void RegisterNullHandler<TPayload>() {
         base.RegisterHandler<TPayload>((envelope) => { });
      }

      protected override void Invoke(Action<InboundEnvelope> handler, InboundEnvelope envelope) {
         handler(envelope);
      }
   }
}
