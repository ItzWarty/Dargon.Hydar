using System.Threading;
using Dargon.Hydar.PortableObjects;
using ItzWarty;
using ItzWarty.Collections;

namespace Dargon.Hydar.Networking {
   public class TestNetwork : Network {
      private readonly TestNetworkConfiguration configuration;
      private readonly IConcurrentSet<NetworkNode> nodes = new ConcurrentSet<NetworkNode>();
      private readonly ConcurrentDictionary<NetworkNode, IRemoteIdentity> identitiesByNode = new ConcurrentDictionary<NetworkNode, IRemoteIdentity>();
      private int addressCounter = 0; 

      public TestNetwork(TestNetworkConfiguration configuration) {
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
         if (StaticRandom.NextDouble() < configuration.PacketLossProbability) {
            return;
         }
         var senderIdentity = identitiesByNode[sender];
         foreach (var node in nodes) {
            if (node == sender) {
               continue;
            }
            if (StaticRandom.NextDouble() < configuration.PacketLossProbability) {
               return;
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