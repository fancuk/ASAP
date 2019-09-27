using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.MainSocket;

namespace TelerikWpfApp3
{
    class SocketController
    {
        public SocketMaker sMaker = new SocketMaker();
        public SocketConnecter sConnect = new SocketConnecter();
        public SocketSend sSend = new SocketSend();
        public SocketReceived sReceive = new SocketReceived();

        public static Socket mSock = null;

        public SocketController()
        {

        }

        public void CloseSocket()
        {
            SendData("<FIN>", "close");
            sMaker.closeSock();
            ((App)Application.Current).nowConnect = false;
        }

        public void StartSocket()
        {
            if (((App)Application.Current).nowConnect == true) return;
            sMaker.UserName = "Winterlood";
            mSock = sMaker.makeSock();

            if (sConnect.StartConnect())
            {
                MessageBox.Show("Success to Socket Connection!");
                ((App)Application.Current).nowConnect = true;
            }
            else
            {
                MessageBox.Show("Socket Connection Failed!");
                ((App)Application.Current).nowConnect = false;
            }
        }
        public void SendData(string type,string text)
        {
            if (((App)Application.Current).nowConnect == false) StartSocket();
            sSend.OnSendData(type, text);
        }
        public void SendData(string text)
        {
            if (((App)Application.Current).nowConnect == false) StartSocket();
            sSend.OnSendData("<MSG>", text);
        }
    }
}
