using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Caching.Proposals.Messages {
   public interface IProposalMessage {
      Guid CacheId { get; }
      Guid ProposalId { get; }
   }
}
