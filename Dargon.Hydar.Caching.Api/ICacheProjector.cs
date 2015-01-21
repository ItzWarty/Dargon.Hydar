namespace Dargon.Hydar.Caching {
   public interface ICacheProjector {}

   public interface ICacheProjector<TKey, TValue, TProjection> : ICacheProjector {
      TProjection Project(ReadableEntry<TKey, TValue> entry);
   }
}
