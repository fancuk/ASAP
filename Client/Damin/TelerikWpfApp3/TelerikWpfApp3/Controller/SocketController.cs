using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TelerikWpfApp3
{
    class SocketController
    {
        MainSock Msock = new MainSock();
        public SocketController()
        {

        }
        public void CloseSocket()
        {
            SendData("<FIN>", "close");
            Msock.closeSock();
            ((App)Application.Current).nowConnect = false;
        }
        public void StartSocket()
        {
            if (((App)Application.Current).nowConnect == true) return;
            Msock.UserName = "Winterlood";
            Msock.makeSock();
            if (Msock.StartConnect())
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
            Msock.OnSendData(type, text);
        }
        public void SendData(string text)
        {
            if (((App)Application.Current).nowConnect == false) StartSocket();
            Msock.OnSendData("<MSG>", text);
        }
    }
}
