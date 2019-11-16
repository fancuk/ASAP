using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.Service;

namespace TelerikWpfApp3.Networking
{
    class SocketCloser
    {
        NetworkManager networkManager = ((App)Application.Current).networkManager;
         private Socket nowSock;
        public SocketCloser()
        {
            nowSock = networkManager.ProgramSock;
        }
        public void closeSock()
        {
            if (networkManager.nowConnect == true)
            {
                networkManager.nowConnect = false;
                nowSock.Dispose();
                nowSock.Close();
                nowSock = null;
            }
        }

    }
}
