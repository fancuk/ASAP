using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.Networking
{
    class SocketMaker
    {
        public Socket makeSock( Socket nowSock)
        {
            nowSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            return nowSock;
        }
    }
}
