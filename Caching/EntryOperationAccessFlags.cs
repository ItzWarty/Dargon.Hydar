using System;

namespace Dargon.Hydar.Caching {
   [Flags]
   public enum EntryOperationAccessFlags : byte {
      Read = 0x01,
      Write = 0x02,
      ReadWrite = Read | Write
   }
}