namespace Dargon.Hydar.Caching {
   public interface CacheConfiguration {
      int Redundancy { get; }
   }

   public class CacheConfigurationImpl : CacheConfiguration {
      private int redundancy = 3;
      public int Redundancy { get { return redundancy; } set { redundancy = value; } }
   }
}
