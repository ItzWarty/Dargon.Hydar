using System;

namespace Dargon.Hydar.Caching {
   [Flags]
   public enum CacheOperationAccessFlags : byte {
      Read = 0x01,
      Write = 0x02,
      ReadWrite = Read | Write
   }
}