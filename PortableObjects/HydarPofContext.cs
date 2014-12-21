using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public class HydarPofContext : PofContext {
      private const int kBasePofId = 10000;

      public HydarPofContext() {
         // [0, 100) base networking stuff
         RegisterPortableObjectType(kBasePofId + 0, typeof(HydarMessageImpl<>));
         RegisterPortableObjectType(kBasePofId + 1, typeof(HydarMessageHeaderImpl));

         // [100, 200) clustering suff
         RegisterPortableObjectType(kBasePofId + 100, typeof(ElectionVote));
         RegisterPortableObjectType(kBasePofId + 101, typeof(LeaderHeartBeat));
         RegisterPortableObjectType(kBasePofId + 102, typeof(MemberHeartBeat));
      }
   }
}
