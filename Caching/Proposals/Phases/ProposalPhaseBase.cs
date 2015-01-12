using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Caching.Proposals.Messages;
using Dargon.Hydar.PortableObjects;
using Dargon.Hydar.Utilities;

namespace Dargon.Hydar.Caching.Proposals.Phases {
   public abstract class ProposalPhaseBase<K, V> : IProposalPhase<K, V> {
      public virtual void Initialize() { }
      public virtual void HandleEnter() { }
      public virtual void Step() { }

      public abstract void ProcessLeaderPrepare(InboundEnvelopeHeader header, ProposalLeaderPrepare<K> message, object activeProposalContextsByEntryKey);
      public abstract void ProcessFollowerAccept(InboundEnvelopeHeader header, ProposalFollowerAccept message);
      public abstract void ProcessFollowerReject(InboundEnvelopeHeader header, ProposalFollowerReject message);
      public abstract void ProcessLeaderCommit(InboundEnvelopeHeader header, ProposalLeaderCommit message);
      public abstract void ProcessLeaderCancel(InboundEnvelopeHeader header, ProposalLeaderCancel message);
   }
}
