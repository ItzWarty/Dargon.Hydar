using System.Runtime.CompilerServices;

namespace Dargon.Hydar.Utilities {
   public class HashUtilities {
      /// <summary>
      /// Robert Jenkins' 32 bit integer hash function.
      /// 
      /// See http://www.cris.com/~Ttwang/tech/inthash.htm 
      /// 
      /// Discovered via https://gist.github.com/badboy/6267743 
      ///            and http://burtleburtle.net/bob/hash/integer.html
      /// 
      /// Licensed under Public Domain.
      /// </summary>
      /// <param name="hash"></param>
      /// <returns></returns>
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static int Mix(int hash) {
         return unchecked((int)Mix((uint)hash));
      }

      /// <summary>
      /// Robert Jenkins' 32 bit integer hash function.
      /// 
      /// See http://www.cris.com/~Ttwang/tech/inthash.htm 
      /// 
      /// Discovered via https://gist.github.com/badboy/6267743 
      ///            and http://burtleburtle.net/bob/hash/integer.html
      /// 
      /// Licensed under Public Domain.
      /// </summary>
      /// <param name="hash"></param>
      /// <returns></returns>
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static uint Mix(uint hash) {
         hash = (hash + 0x7ed55d16) + (hash << 12);
         hash = (hash ^ 0xc761c23c) ^ (hash >> 19);
         hash = (hash + 0x165667b1) + (hash << 5);
         hash = (hash + 0xd3a2646c) ^ (hash << 9);
         hash = (hash + 0xfd7046c5) + (hash << 3);
         hash = (hash ^ 0xb55a4f09) ^ (hash >> 16);
         return hash;
      }
   }
}
