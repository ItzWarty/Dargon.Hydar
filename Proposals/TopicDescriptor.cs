using System;

namespace Dargon.Hydar.Proposals {
   public interface TopicDescriptor {
      Guid Id { get; }
      string Name { get; }
   }
}