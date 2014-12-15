namespace Dargon.Hydar.Networking {
   class TestRemoteIdentity : IRemoteIdentity {
      private readonly uint address;

      public TestRemoteIdentity(uint address) {
         this.address = address;
      }

      public uint Address { get { return address; } }
   }
}