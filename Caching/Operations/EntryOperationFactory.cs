using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar.Caching.Operations {
   public interface EntryOperationFactory {
      EntryOperation<K, V> Read<K, V>();
      EntryOperation<K, V> Write<K, V>(V value);
   }

   public class EntryOperationFactoryImpl : EntryOperationFactory {
      public EntryOperation<K, V> Read<K, V>() {
         return new EntryReadOperation<K, V>();
      }

      public EntryOperation<K, V> Write<K, V>(V value) {
         return new EntryWriteOperation<K, V>(value);
      }
   }
}
