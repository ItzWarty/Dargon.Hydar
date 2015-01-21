using Dargon.PortableObjects;

namespace Dargon.Hydar.Caching.Processors {
   public class PutEntryProcessor<TKey, TValue> : EntryProcessor<TKey, TValue, bool>, IPortableObject {
      private TValue value;

      public PutEntryProcessor() { } 

      public PutEntryProcessor(TValue value) {
         this.value = value;
      }

      public EntryAccessFlags AccessFlags { get { return EntryAccessFlags.Write; } }

      public bool Process(ManageableEntry<TKey, TValue> entry) {
         entry.Value = value;
         return true;
      }

      public void Serialize(IPofWriter writer) {
         writer.WriteObject(0, value);
      }

      public void Deserialize(IPofReader reader) {
         value = reader.ReadObject<TValue>(0);
      }
   }
}