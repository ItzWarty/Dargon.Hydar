namespace Dargon.Hydar.Caching {
   public class PartitionRange {
      private readonly int startInclusive;
      private readonly int endExclusive;

      public PartitionRange(int startInclusive, int endExclusive) {
         this.startInclusive = startInclusive;
         this.endExclusive = endExclusive;
      }

      public int StartInclusive { get { return startInclusive; } }
      public int EndExclusive { get { return endExclusive; } }
      public int Count { get { return endExclusive - startInclusive; } }

      public override string ToString() {
         return "[" + startInclusive + ", " + endExclusive + ")";
      }
   }
}