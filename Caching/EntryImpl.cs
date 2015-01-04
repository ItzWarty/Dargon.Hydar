namespace Dargon.Hydar.Caching {
   public class EntryImpl<K, V> : ManageableEntry<K, V> {
      private readonly K key;
      private V value;
      private bool present;
      private bool removed;
      private bool dirty;

      public EntryImpl() { } 

      public EntryImpl(K key) : this(key, default(V), false) { }

      public EntryImpl(K key, V value, bool present) {
         this.key = key;
         this.value = value;
         this.present = present;
      }

      public K Key { get { return key; } }
      public V Value { get { return value; } set { this.value = value; } }
      public bool IsDirty { get { return dirty; } set { dirty = value; } }
      public bool IsPresent { get { return present; } }

      public void Remove() {
         removed = true;
      }
   }
}
