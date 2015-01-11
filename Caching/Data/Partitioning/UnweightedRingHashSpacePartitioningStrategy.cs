using System;
using System.Diagnostics;
using System.Linq;
using ItzWarty;
using ItzWarty.Collections;
using SCG = System.Collections.Generic;

namespace Dargon.Hydar.Caching.Data.Partitioning {
   public class UnweightedRingHashSpacePartitioningStrategy : HashSpacePartitioningStrategy {
      private const long kTwoPow32 = 0x100000000L;

      private readonly int blockCount;
      private readonly int redundancy;
      private readonly int hashesPerBlock;

      public UnweightedRingHashSpacePartitioningStrategy(int blockCount, int redundancy) {
         if (blockCount <= 1) {
            throw new ArgumentException("blockCount must be > 1");
         } else if (redundancy < 1) {
            throw new ArgumentException("redundancy must be >= 1");
         } else if (blockCount < redundancy) {
            throw new ArgumentException("blockCount must be < redundancy");
         }

         this.blockCount = blockCount;
         this.redundancy = redundancy;
         this.hashesPerBlock = (int)(kTwoPow32 / blockCount);

         Trace.Assert((long)blockCount * (long)hashesPerBlock == kTwoPow32, "Hash-space must divide evenly.");
      }

      public int BlockCount { get { return blockCount; } }

      public unsafe int GetBlockForHash(int hash) {
         return (int)((*(uint*)&hash) / hashesPerBlock);
      }

      public BlockDescriptor GetBlockDescriptor(int blockId) {
         return new BlockDescriptor(
            hashesPerBlock * blockId,
            hashesPerBlock
         );
      }

      public SCG.IReadOnlyDictionary<Guid, IReadOnlySet<BlockPartitionRange>> Partition(IReadOnlySet<Guid> nodeGuidsInput) {
         var nodeGuids = nodeGuidsInput.ToArray().With(Array.Sort);
         var partitionRanges = Util.Generate(nodeGuids.Length, i => {
            var startInclusive = i == 0 ? 0 : (int)(blockCount * ((float)i / nodeGuids.Length));
            var endExclusive = i == nodeGuids.Length - 1 ? blockCount : (int)(blockCount * ((float)(i + 1) / nodeGuids.Length));
            return new BlockPartitionRange(startInclusive, endExclusive);
         });
         var result = new SCG.Dictionary<Guid, IReadOnlySet<BlockPartitionRange>>();
         for (var i = 0; i < nodeGuids.Length; i++) {
            var partitions = new ItzWarty.Collections.HashSet<BlockPartitionRange>();
            for (var r = 0; r < redundancy; r++) {
               partitions.Add(partitionRanges[(i + r) % partitionRanges.Length]);
            }
            result.Add(nodeGuids[i], partitions);
         }
         return result;
      }
   }
}