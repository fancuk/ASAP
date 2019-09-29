using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TelerikWpfApp3.Networking
{
    class SocketCloser
    {
        private Socket nowSock;
        public SocketCloser()
        {
            nowSock = ((App)Application.Current).ProgramSock;
        }
        public void closeSock()
        {
            if (((App)Application.Current).nowConnect == true)
            {
                ((App)Application.Current).nowConnect = false;
                nowSock.Dispose();
                nowSock.Close();
                nowSock = null;
            }
        }

    }
}
