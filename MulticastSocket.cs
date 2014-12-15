using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Hydar {
   public class MulticastSocket {
      private readonly Socket socket;

      public MulticastSocket(int port, IPAddress groupAddress) {
         socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
         socket.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.ReuseAddress, true);
         socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 0);
         socket.Bind(new IPEndPoint(IPAddress.Any, port));
         socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(groupAddress));
      }
   }
}
