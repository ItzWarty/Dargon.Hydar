namespace Dargon.Hydar.Caching {
   public interface CacheConfiguration {
      int Redundancy { get; }
      HashSpacePartitioningStrategy PartitioningStrategy { get; }
      int BlockSize { get; }
      int BlockCount { get; }
   }

   public class CacheConfigurationImpl : CacheConfiguration {
      private const long kHashSpaceSize = 1L << 32;

      private int redundancy = 3;
      private int blockSize = 1 << 18;
      private HashSpacePartitioningStrategy partitioningStrategy = new UnweightedRingHashSpacePartitioningStrategy();

      public int Redundancy { get { return redundancy; } set { redundancy = value; } }
      public HashSpacePartitioningStrategy PartitioningStrategy { get { return partitioningStrategy; } set { partitioningStrategy = value; } }
      public int BlockSize { get { return blockSize; } set { blockSize = value; } }
      public int BlockCount { get { return (int)(kHashSpaceSize / (long)blockSize); } set { blockSize = (int)((double)kHashSpaceSize / value); } }
   }
}
