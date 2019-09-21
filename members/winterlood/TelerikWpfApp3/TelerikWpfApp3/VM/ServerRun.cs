using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TelerikWpfApp3.M;

namespace TelerikWpfApp3.VM
{
    public class ServerRun : INotifyPropertyChanged
    {
        MainSock mainSock = null;
        private string _serverStatus;
        private string _msgTextBox;

        public string msgTextBox
        {
            get
            {
                return this._msgTextBox;
            }
            set
            {
                this._msgTextBox = value;
                OnPropertyChanged(msgTextBox);
            }
        }
        public string serverStatus
        {
            get
            {
                return this._serverStatus;
            }
            set
            {
                this._serverStatus = value;
                OnPropertyChanged("serverStatus");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

   

        private bool nowListen = false;
        public ICommand ServerStart { get; set; }
        public ICommand SendText { get; set; }
        public ICommand Func1  { get; set;}
        public ServerRun()
        {
            ServerStart = new Command(ExecuteServerRun, CanExecuteMethod);
            Func1 = new Command(ExecuteMethod2, CanExecuteMethod);
            SendText = new Command(ExeceuteSendMsg,CanExecuteMethod);
            serverStatus = "now Server is not Running";
        }

        private void ExecuteServerRun(object obj)
        {
            if (this.nowListen == true)
            {
                MessageBox.Show("Server is Already Run!!");
                return;
            }
            else
            {
                mainSock = new MainSock();
                mainSock.mSock=mainSock.makeSock();
                mainSock.bindSock(mainSock.mSock);
                mainSock.RunServer(mainSock.mSock);
                this.nowListen = true;
                serverStatus = "now Server is Running";
            }
        }

        private void ExeceuteSendMsg(object obj)
        {
            if (this.nowListen == true)
            {
                //mainSock.OnSendData(obj.ToString());
                msgTextBox = "";
            }
            else
            {
                MessageBox.Show("Server connect plase");
            }
        }

        private void ExecuteMethod(object obj)
        {
            MessageBox.Show("Button Click!");
        }
        private void ExecuteMethod2(object obj, object obj2)
        {
            MessageBox.Show(obj.ToString() + obj2.ToString());
        }
        private bool CanExecuteMethod(object arg)
        {
            return true;
        }
    }
}
