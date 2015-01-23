namespace Dargon.Hydar.Caching.Operations {
   public interface EntryOperationFactory {
      ManageableEntryOperation<K, V> Read<K, V>();
      ManageableEntryOperation<K, V> Write<K, V>(V value);
   }

   public class EntryOperationFactoryImpl : EntryOperationFactory {
      public ManageableEntryOperation<K, V> Read<K, V>() {
         return new EntryReadOperation<K, V>();
      }

      public ManageableEntryOperation<K, V> Write<K, V>(V value) {
         return new EntryWriteOperation<K, V>(value);
      }
   }
}
