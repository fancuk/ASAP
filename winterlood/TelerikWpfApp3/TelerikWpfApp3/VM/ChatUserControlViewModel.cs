using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TelerikWpfApp3.M;
using TelerikWpfApp3;
using System.Windows.Documents;
using System.Collections.ObjectModel;

namespace TelerikWpfApp3.VM
{
    class ChatUserControlViewModel:INotifyPropertyChanged
    {
        MainSock mainSock = null;
        private string _serverStatus;
        private string _msgTextBox;
        private bool nowListen = false;
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
        
        object cl = ((App)Application.Current).getChat();

        public ICommand serverRun { get; set; }
        public ICommand SendText { get; set; }

        public ChatUserControlViewModel()
        {
            serverRun = new Command(ExecuteServerRun, CanExecuteMethod);
            SendText = new Command(ExeceuteSendMsg, CanExecuteMethod);

        }

        private void ExecuteConnectServer(object obj)
        {
            
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
                mainSock.mSock = mainSock.makeSock();
                mainSock.bindSock(mainSock.mSock);
                mainSock.RunServer(mainSock.mSock);
                this.nowListen = true;
                serverStatus = "now Server is Running";
               
            }
        }

        private void ExeceuteSendMsg(object obj)
        {
           ((App)Application.Current).SendData(obj as string);
            msgTextBox = "";
        }
        private bool CanExecuteMethod(object arg)
        {
            return true;
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
