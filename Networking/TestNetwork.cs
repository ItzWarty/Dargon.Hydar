using System.IO;
using System.Text;
using Dargon.Hydar.PortableObjects;
using ItzWarty;
using ItzWarty.Collections;
using System.Threading;
using Dargon.PortableObjects;

namespace Dargon.Hydar.Networking {
   public class TestNetwork : Network {
      private readonly IPofSerializer pofSerializer;
      private readonly TestNetworkConfiguration configuration;
      private readonly IConcurrentSet<NetworkNode> nodes = new ConcurrentSet<NetworkNode>();
      private readonly ConcurrentDictionary<NetworkNode, IRemoteIdentity> identitiesByNode = new ConcurrentDictionary<NetworkNode, IRemoteIdentity>();
      private int addressCounter = 0; 

      public TestNetwork(IPofSerializer pofSerializer, TestNetworkConfiguration configuration) {
         this.pofSerializer = pofSerializer;
         this.configuration = configuration;
      }

      public void Join(NetworkNode node) {
         var address = (uint)Interlocked.Increment(ref addressCounter);
         identitiesByNode.TryAdd(node, new TestRemoteIdentity(address));
         nodes.Add(node);
      }

      public void Part(NetworkNode node) {
         nodes.Remove(node);
      }

      public void Broadcast(NetworkNode sender, HydarMessage message) {
         using (var ms = new MemoryStream()) {
            using (var writer = new BinaryWriter(ms, Encoding.UTF8, true)) {
               pofSerializer.Serialize(writer, message);
            }
            ms.Position = 0;
            using (var reader = new BinaryReader(ms, Encoding.UTF8, true)) {
               message = pofSerializer.Deserialize<HydarMessage>(reader);
            }
         }

         if (StaticRandom.NextDouble() < configuration.PacketLossProbability) {
            return;
         }
         var senderIdentity = identitiesByNode[sender];
         foreach (var node in nodes) {
            if (node == sender) {
               continue;
            }
            if (StaticRandom.NextDouble() < configuration.PacketLossProbability) {
               continue;
            }
            var delay = configuration.PacketReorderingFactor * StaticRandom.NextDouble();
            var capture = node;
            var timer = new Timer((e) => {
               capture.Receive(senderIdentity, message);
            }, null, (int)delay, -1);
         }
      }
   }
}