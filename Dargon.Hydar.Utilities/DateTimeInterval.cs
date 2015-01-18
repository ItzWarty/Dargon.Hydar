using Dargon.PortableObjects;
using System;

namespace Dargon.Hydar.Utilities {
   public class DateTimeInterval : IPortableObject {
      public DateTimeInterval() { }

      public DateTimeInterval(DateTime start, DateTime end) {
         Start = start;
         End = end;
      }

      public DateTimeInterval(DateTime start, TimeSpan span) {
         Start = start;
         End = start + span;
      }

      public DateTime Start { get; set; }
      public DateTime End { get; set; }
      public TimeSpan Span { get { return End - Start; } set { End = Start + value; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteS64(0, Start.ToBinary());
         writer.WriteS64(1, End.ToBinary());
      }

      public void Deserialize(IPofReader reader) {
         Start = DateTime.FromBinary(reader.ReadS64(0));
         End = DateTime.FromBinary(reader.ReadS64(1));
      }
   }
}
