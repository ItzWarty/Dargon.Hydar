using System;
using ItzWarty.Collections;
using SCG = System.Collections.Generic;

namespace Dargon.Hydar.Caching {
   public interface HashSpacePartitioningStrategy {
      SCG.IReadOnlyDictionary<Guid, IReadOnlySet<PartitionRange>> Partition(IReadOnlySet<Guid> nodeGuids, int blockCount, int redundancy);
   }
}