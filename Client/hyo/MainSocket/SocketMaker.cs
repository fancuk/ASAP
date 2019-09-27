using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TelerikWpfApp3.MainSocket
{
    public class SocketMaker
    {
  
        private string userName;

        delegate void STMW();
        public string UserName
        {
            get { return this.userName; }
            set
            {
                this.userName = value;
            }
        }
        public SocketMaker()
        {

        }
        public Socket makeSock()
        {
            SocketController.mSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            return SocketController.mSock;
        }

        public void closeSock()
        {
            if (((App)Application.Current).nowConnect == true)
            {
                SocketController.mSock.Dispose();
                SocketController.mSock.Close();
                SocketController.mSock = null;
                ((App)Application.Current).nowConnect = false;
            }
        }

        public Socket bindSock(Socket sock)
        {
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, 11000);
            sock.Bind(serverEP);
            return sock;
        }
    }
}
