﻿namespace Dargon.Hydar.Caching.Operations {
   public abstract class ReturningEntryOperation<K, V, R> : EntryOperationBase<K, V> {
      private R result;

      public abstract override EntryAccessFlags AccessFlags { get; }

      protected override void Execute(ManageableEntry<K, V> entry) {
         result = ExecuteInternal(entry);
      }

      public abstract R ExecuteInternal(ManageableEntry<K, V> entry);

      public R GetResult() {
         Wait();
         return result;
      }
   }
}
