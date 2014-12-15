namespace Dargon.Hydar.Networking {
   public class TestNetworkConfiguration {
      private double packetLossProbability = 0.1;
      private double packetReorderingFactor = 200;

      /// <summary>
      /// Probability from 0 to 1 of our test network dropping a udp packet.
      /// </summary>
      public double PacketLossProbability { get { return packetLossProbability; } set { packetLossProbability = value; } }

      /// <summary>
      /// Number of milliseconds in latency we may wait before "sending" a packet.
      /// When multiple packets are sent in succession, this can lead to reordering.
      /// </summary>
      public double PacketReorderingFactor { get { return packetReorderingFactor; } set { packetReorderingFactor = value; } }
   }
}