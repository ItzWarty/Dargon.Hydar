using Dargon.Hydar.Caching.Operations;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Caching {
   public class HydarCachingImplPofContext : PofContext {
      private const int kBasePofId = 2400;

      public HydarCachingImplPofContext() {
         RegisterPortableObjectType(kBasePofId + 0, typeof(EntryReadOperation<,>));
         RegisterPortableObjectType(kBasePofId + 1, typeof(EntryWriteOperation<,>));
         RegisterPortableObjectType(kBasePofId + 2, typeof(EntryProcessOperation<,,>));
      }
   }
}
