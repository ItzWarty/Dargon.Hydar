using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Proposals.Messages.Helpers;

namespace Dargon.Hydar.Proposals.Messages {
   public interface AtomicProposalReject : AtomicProposalMessage {
      RejectionReason RejectionReason { get; }
   }
}
