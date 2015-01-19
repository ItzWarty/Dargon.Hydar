using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Proposals {
   public class TopicDescriptorImpl : TopicDescriptor {
      private readonly Guid id;
      private readonly string name;

      public TopicDescriptorImpl(Guid id, string name) {
         this.id = id;
         this.name = name;
      }

      public Guid Id { get { return id; } }
      public string Name { get { return name; } }
   }
}
