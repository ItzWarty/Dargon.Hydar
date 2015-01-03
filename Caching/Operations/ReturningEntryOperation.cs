using System.Threading;

namespace Dargon.Hydar.Caching.Operations {
   public abstract class ReturningEntryOperation<K, V, R> : EntryOperationBase<K, V> {
      private R result;
      private readonly CountdownEvent completionLatch = new CountdownEvent(1);

      public abstract override EntryOperationAccessFlags AccessFlags { get; }

      protected override void Execute(ManageableEntry<K, V> entry) {
         result = ExecuteInternal(entry);
      }

      public abstract R ExecuteInternal(ManageableEntry<K, V> entry);
   }
}
