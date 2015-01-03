namespace Dargon.Hydar.Caching {
   public interface ReadableEntry<K, V> {
      K Key { get; }
      V Value { get; }
      bool IsPresent { get; }
   }

   public interface ManageableEntry<K, V> : ReadableEntry<K, V> {
      new V Value { get; set; }
   }

   public class EntryImpl<K, V> : ManageableEntry<K, V> {
      private readonly K key;
      private V value;
      private bool present;

      public EntryImpl() { } 

      public EntryImpl(K key) : this(key, default(V), false) { }

      public EntryImpl(K key, V value, bool present) {
         this.key = key;
         this.value = value;
         this.present = present;
      }

      public K Key { get { return key; } }
      public V Value { get { return value; } set { this.value = value; } }
      public bool IsPresent { get { return present; } }
   }
}
