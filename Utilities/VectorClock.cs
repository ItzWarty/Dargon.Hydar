using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dargon.PortableObjects;
using ItzWarty;

namespace Dargon.Hydar.Utilities {
   public class VectorClock : IPortableObject {
      private int[] counts;

      public VectorClock() { }

      public VectorClock(int elements) {
         counts = new int[elements];
      }

      private VectorClock(int[] counts) {
         this.counts = counts;
      }

      public IReadOnlyList<int> Counts { get { return counts; } }

      public int Increment(int index) {
         return Interlocked.Increment(ref counts[index]);
      }

      public VectorClock Clone() {
         return new VectorClock(TakeCountsSnapshot());
      }

      public void Serialize(IPofWriter writer) {
         writer.WriteCollection(0, TakeCountsSnapshot());
      }

      public void Deserialize(IPofReader reader) {
         counts = reader.ReadArray<int>(0);
      }

      private int[] TakeCountsSnapshot() {
         return Util.Generate(counts.Length, i => Interlocked.CompareExchange(ref counts[i], 0, 0));
      }
   }
}
