using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.PortableObjects;

namespace Dargon.Hydar.PortableObjects {
   public class AccountEntry : IPortableObject {
      private Guid id;
      private string name;
      private string email;
      private Guid passwordHash; // we use GUIDs because byte array serialization isn't optimized yet.

      public AccountEntry() { }

      public AccountEntry(Guid id, string name, string email, Guid passwordHash) {
         this.id = id;
         this.name = name;
         this.email = email;
         this.passwordHash = passwordHash;
      }

      public Guid Id { get { return id; } }
      public string Name { get { return name; } }
      public string Email { get { return email; } }
      public Guid PasswordHash { get { return passwordHash; } }

      public void Serialize(IPofWriter writer) {
         writer.WriteGuid(0, id);
         writer.WriteString(1, name);
         writer.WriteString(2, email);
         writer.WriteGuid(3, passwordHash);
      }

      public void Deserialize(IPofReader reader) {
         id = reader.ReadGuid(0);
         name = reader.ReadString(1);
         email = reader.ReadString(2);
         passwordHash = reader.ReadGuid(3);
      }
   }
}
