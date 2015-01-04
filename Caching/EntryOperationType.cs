using System;

namespace Dargon.Hydar.Caching {
   public enum EntryOperationType : byte {
      Read = 1,
      Write = 2,
      Update = 3
   }
}