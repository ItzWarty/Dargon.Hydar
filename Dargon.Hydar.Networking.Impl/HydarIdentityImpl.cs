using System;
using SCG = System.Collections.Generic;

namespace Dargon.Hydar.Networking {
   public class HydarIdentityImpl : HydarIdentity {
      private readonly Guid nodeId;
      private readonly ItzWarty.Collections.ISet<Guid> groupIds = new ItzWarty.Collections.HashSet<Guid>(); 
      private readonly SCG.Dictionary<Guid, string> groupNameById = new SCG.Dictionary<Guid, string>(); 

      public HydarIdentityImpl(Guid nodeId) {
         this.nodeId = nodeId;
      }

      public Guid NodeId { get { return nodeId; } }
      public ItzWarty.Collections.ISet<Guid> GroupIds { get { return groupIds; } } 
      public SCG.IReadOnlyDictionary<Guid, string> GroupsByName { get { return groupNameById; } } 

      public void AddGroup(Guid id, string name) {
         groupIds.Add(id);
         groupNameById.Add(id, name);
      }

      public void RemoveGroup(Guid id) {
         groupIds.Remove(id);
         groupNameById.Remove(id);
      }
   }
}
