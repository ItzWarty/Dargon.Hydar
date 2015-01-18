namespace Dargon.Hydar.Networking {
   public class TestRemoteIdentity {
      private readonly uint address;

      public TestRemoteIdentity(uint address) {
         this.address = address;
      }

      public uint Address { get { return address; } }
   }
}