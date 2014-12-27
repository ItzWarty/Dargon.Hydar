using Dargon.Hydar.PortableObjects;
using Dargon.PortableObjects;
using ItzWarty;
using ItzWarty.Collections;
using System.IO;
using System.Text;
using System.Threading;

namespace Dargon.Hydar.Networking {
   public class TestNetworkManager {
      private readonly IPofSerializer pofSerializer;
      private readonly TestNetworkConfiguration configuration;
      private readonly TestNetworkUtilities utilities;
      private readonly IConcurrentSet<TestNetworkInstance> connectedNetworkInstances = new ConcurrentSet<TestNetworkInstance>();

      public TestNetworkManager(IPofSerializer pofSerializer, TestNetworkConfiguration configuration) {
         this.pofSerializer = pofSerializer;
         this.configuration = configuration;
         this.utilities = new TestNetworkUtilities(configuration);
      }

      public void Join(TestNetworkInstance instance) {
         connectedNetworkInstances.Add(instance);
      }

      public void Part(TestNetworkInstance instance) {
         connectedNetworkInstances.Remove(instance);
      }

      public TestNetworkInstance CreateNetworkInstance() {
         var address = utilities.NextAddress();
         var identity = new TestRemoteIdentity(address);
         return new TestNetworkInstance(this, pofSerializer, utilities, identity);
      }

      private void BroadcastInternal(TestRemoteIdentity senderIdentity, OutboundEnvelope envelope) {
         foreach (var instance in connectedNetworkInstances) {
            if (instance.Identity == senderIdentity || utilities.IsDroppedPacket()) {
               continue;
            }

            var delay = utilities.GetRandomDelay();
            var capture = instance;
            var timer = new Timer((e) => {
               capture.Receive(senderIdentity, envelope);
            }, null, (int)delay, -1);
         }
      }

      public class TestNetworkInstance : Network {
         private readonly TestNetworkManager parent;
         private readonly IPofSerializer pofSerializer;
         private readonly TestNetworkUtilities utilities;
         private readonly TestRemoteIdentity identity;
         public event EnvelopeArrivedHandler EnvelopeArrived;

         public TestNetworkInstance(TestNetworkManager parent, IPofSerializer pofSerializer, TestNetworkUtilities utilities, TestRemoteIdentity identity) {
            this.parent = parent;
            this.pofSerializer = pofSerializer;
            this.utilities = utilities;
            this.identity = identity;
         }

         public TestRemoteIdentity Identity { get { return identity; } }

         public void Receive(TestRemoteIdentity senderIdentity, OutboundEnvelope envelope) {
            var capture = EnvelopeArrived;
            if (capture != null) {
               var inboundEnvelope = envelope.ToInboundEnvelope(senderIdentity.Address);
               capture(this, inboundEnvelope);
            }
         }

         public void Broadcast(OutboundEnvelope envelope) {
            SerializeAndDeserialize(envelope);
            if (!utilities.IsDroppedPacket()) {
               parent.BroadcastInternal(identity, envelope);
            }
         }

         private void SerializeAndDeserialize(IPortableObject obj) {
            using (var ms = new MemoryStream()) {
               using (var writer = new BinaryWriter(ms, Encoding.UTF8, true)) {
                  pofSerializer.Serialize(writer, obj);
               }
               ms.Position = 0;
               using (var reader = new BinaryReader(ms, Encoding.UTF8, true)) {
                  pofSerializer.Deserialize(reader);
               }
            }
         }
      }

      public class TestNetworkUtilities {
         private readonly TestNetworkConfiguration configuration;

         private int addressCounter = 0;

         public TestNetworkUtilities(TestNetworkConfiguration configuration) {
            this.configuration = configuration;
         }

         public uint NextAddress() {
            return (uint)Interlocked.Increment(ref addressCounter);
         }

         public bool IsDroppedPacket() {
            return StaticRandom.NextDouble() < configuration.PacketLossProbability;
         }

         public double GetRandomDelay() {
            return configuration.PacketReorderingFactor * StaticRandom.NextDouble();
         }
      }
   }
}