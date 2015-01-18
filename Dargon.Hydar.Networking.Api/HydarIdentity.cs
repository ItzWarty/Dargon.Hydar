using System;
using SCG = System.Collections.Generic;

namespace Dargon.Hydar.Networking {
    public interface HydarIdentity {
       Guid NodeId { get; }
       ItzWarty.Collections.ISet<Guid> GroupIds { get; }
       SCG.IReadOnlyDictionary<Guid, string> GroupsByName { get; }
    
       void AddGroup(Guid id, string name);
       void RemoveGroup(Guid id);
    }
}