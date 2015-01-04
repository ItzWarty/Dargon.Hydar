using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Hydar.Caching;

namespace DummyApplication.Hydar {
   public class ToLowerProcessor : EntryProcessor<int, string, string> {
      public string Process(ManageableEntry<int, string> entry) {
         if (entry.Value != null) {
            entry.Value = entry.Value.ToLower();
         }
         return entry.Value;
      }
   }
}
