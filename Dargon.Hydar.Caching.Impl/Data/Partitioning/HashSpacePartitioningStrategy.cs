using System;
using ItzWarty.Collections;
using SCG = System.Collections.Generic;

namespace Dargon.Hydar.Caching.Data.Partitioning {
   public interface HashSpacePartitioningStrategy {
      int BlockCount { get; }

      int GetBlockForHash(int hash);
      BlockDescriptor GetBlockDescriptor(int blockId);
      SCG.IReadOnlyDictionary<Guid, IReadOnlySet<BlockPartitionRange>> Partition(IReadOnlySet<Guid> nodeGuids);
   }
}