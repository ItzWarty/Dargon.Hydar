namespace Dargon.Hydar.Caching.Data {
   public class BlockPartitionRange {
      private readonly int startInclusive;
      private readonly int endExclusive;

      public BlockPartitionRange(int startInclusive, int endExclusive) {
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