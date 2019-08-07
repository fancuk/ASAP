using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Sockets;
using System.Net;
using System.Windows.Threading;
using System.Windows;

namespace Server
{
    class ViewModel : INotifyPropertyChanged
    {
        #region Servertext
        private string Servertext = "ServerClosed";
        public string ServerText
        {
            get { return this.Servertext; }
            set
            {
                this.Servertext = value;
                OnpropertyChanged("ServerText");
            }
        }
        #endregion
        bool isItRunning = false;
       

        public ICommand serverButton { get; set; }

        private void Serverun(object obj)
        {
            if (isItRunning)
            {
                MessageBox.Show("서버가 켜져있습니다.");
            }
            else
            {
                Socket Server = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.IP);
                Server.Bind(new IPEndPoint(IPAddress.Any, 10801));
                Server.Listen(100);
                isItRunning = true;
                ServerText = "Server Open";
                MessageBox.Show("서버 오픈");
            }
        }

        public ViewModel()
        {
            serverButton = new Command(Serverun);
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnpropertyChanged(string str)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }
        #endregion
    }
}
