using ItzWarty;
using ItzWarty.Collections;
using System;
using System.Linq;

namespace Dargon.Hydar.Caching {
   public class UnweightedRingHashSpacePartitioningStrategy : HashSpacePartitioningStrategy {
      public System.Collections.Generic.IReadOnlyDictionary<Guid, IReadOnlySet<PartitionRange>> Partition(IReadOnlySet<Guid> nodeGuidsInput, int blockCount, int redundancy) {
         var nodeGuids = nodeGuidsInput.ToArray().With(Array.Sort);
         var partitionRanges = Util.Generate(nodeGuids.Length, i => {
            var startInclusive = i == 0 ? 0 : (int)(blockCount * ((float)i / nodeGuids.Length));
            var endExclusive = i == nodeGuids.Length - 1 ? blockCount : (int)(blockCount * ((float)(i + 1) / nodeGuids.Length));
            return new PartitionRange(startInclusive, endExclusive);
         });
         var result = new System.Collections.Generic.Dictionary<Guid, IReadOnlySet<PartitionRange>>();
         for (var i = 0; i < nodeGuids.Length; i++) {
            var partitions = new HashSet<PartitionRange>();
            for (var r = 0; r < redundancy; r++) {
               partitions.Add(partitionRanges[(i + r) % partitionRanges.Length]);
            }
            result.Add(nodeGuids[i], partitions);
         }
         return result;
      }
   }
}