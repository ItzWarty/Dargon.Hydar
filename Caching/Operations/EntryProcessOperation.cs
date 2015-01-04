using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Caching.Operations {
   public class EntryProcessOperation<K, V, R> : ReturningEntryOperation<K, V, R> {
      private readonly IEntryProcessor<K, V, R> entryProcessor;

      public EntryProcessOperation(IEntryProcessor<K, V, R> entryProcessor) {
         this.entryProcessor = entryProcessor;
      }

      public override EntryOperationAccessFlags AccessFlags { get { return EntryOperationAccessFlags.ReadWrite; } }

      public override R ExecuteInternal(ManageableEntry<K, V> entry) {
         entryProcessor.Process(entry)
      }
   }
}
