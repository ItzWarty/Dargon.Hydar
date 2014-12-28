using ItzWarty.Collections;
using System;
using SCG = System.Collections.Generic;

namespace Dargon.Hydar {
    public interface HydarIdentity {
       Guid NodeId { get; }
       ISet<Guid> GroupIds { get; }
       SCG.IReadOnlyDictionary<Guid, string> GroupsByName { get; }
    
       void AddGroup(Guid id, string name);
       void RemoveGroup(Guid id);
    }
}