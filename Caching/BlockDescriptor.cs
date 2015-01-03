namespace Dargon.Hydar.Caching {
   public struct BlockDescriptor {
      private int offset;
      private int length;

      public BlockDescriptor(int offset, int length) {
         this.offset = offset;
         this.length = length;
      }

      public int Offset { get { return offset; } }
      public int Length { get { return length; } }
   }
}