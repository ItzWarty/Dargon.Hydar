using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Caching;
using Dargon.Hydar.Caching.Operations;
using Dargon.Hydar.Caching.Processors;

namespace DummyApplication.Hydar {
   public class ToLowerProcessor : EntryProcessor<int, string, string> {
      public EntryAccessFlags AccessFlags { get { return EntryAccessFlags.Write; } }

      public string Process(ManageableEntry<int, string> entry) {
         if (entry.Value != null) {
            entry.Value = entry.Value.ToLower();
         }
         return entry.Value;
      }
   }
}
