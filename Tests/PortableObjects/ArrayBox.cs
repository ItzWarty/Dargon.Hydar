using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public class ArrayBox<T> : IPortableObject {
      private T[] elements;

      public ArrayBox() { }

      public ArrayBox(T[] elements) {
         this.elements = elements;
      }  

      public T[] Elements { get { return elements; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteCollection(0, elements);
      }

      public void Deserialize(IPofReader reader) {
         elements = reader.ReadArray<T>(0);
      }
   }
}
