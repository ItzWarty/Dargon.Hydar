using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Caching.Processors {
   public class RemoveEntryProcessor<TKey, TValue> : EntryProcessor<TKey, TValue, bool> {
      public EntryAccessFlags AccessFlags { get { return EntryAccessFlags.Write; } }

      public bool Process(ManageableEntry<TKey, TValue> entry) {
         if (entry.IsPresent) {
            entry.Remove();
            return true;
         } else {
            return false;
         }
      }
   }
}
