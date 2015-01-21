namespace Dargon.Hydar.Caching {
   public interface IFilterArgument<out V, out TProjection> {
      V Value { get; }
      TProjection Projection { get; }
   }
}
