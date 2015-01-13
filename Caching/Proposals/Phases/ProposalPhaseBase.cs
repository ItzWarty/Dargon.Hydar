using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public abstract class ProposalPhaseBase : EnvelopeProcessorBase<InboundEnvelope, Action<InboundEnvelope>>, IProposalPhase {
      public virtual void Initialize() { }
      public virtual void HandleEnter() { }
      public virtual void Step() { }
      public virtual void HandleBullied() { }

      protected void RegisterHandler<TMessage>(Action<InboundEnvelopeHeader, TMessage> handler) {
         RegisterHandler<TMessage>(e => handler(e.Header, (TMessage)e.Message));
      }

      protected override void Invoke(Action<InboundEnvelope> handler, InboundEnvelope envelope) {
         handler(envelope);
      }
   }
}
