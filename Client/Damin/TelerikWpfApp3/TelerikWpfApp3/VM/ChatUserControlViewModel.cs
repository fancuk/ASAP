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
using TelerikWpfApp3.VM.DBControl;

namespace TelerikWpfApp3.VM
{
    class ChatUserControlViewModel : INotifyPropertyChanged
    {
        database sqlite = new database();
        private string _msgTextBox;
        private string searchname;
        private string _target;
        public string target
        {
            get
            {
                return this. _target;
            }
            set
            {
                this._target = value;
                OnPropertyChanged(target);
            }
        }
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
        public string searchName
        {
            get { return searchname; }
            set
            {
                searchname = value;
                OnPropertyChanged("searchname");
            }
        }
        public ICommand SendText { get; set; }
        public ICommand friendPlus { get; set; }
        public ChatUserControlViewModel()
        {
            friendPlus = new Command(fpButton, CanExecuteMethod);
            SendText = new Command(ExeceuteSendMsg, CanExecuteMethod);

        }

        public void fpButton(object org)
        {
            string str = searchName + "/";
            string member = ((App)Application.Current).getmyID();
            ((App)Application.Current).SendData("<FRR>", str + member);
        }

        public void ExeceuteSendMsg(object org)
        {
            string id = ((App)Application.Current).getmyID();
            target = ((App)Application.Current).getTarget();
            string plain = org as string;
            string nowTime = DateTime.Now.ToString();
            string msg =  target + "/" + id + "/" + DateTime.Now as string + "/" + plain + "/";
            sqlite.ChattingCreate(id, target, nowTime, plain);
            Chatitem tmp = new Chatitem();
            tmp.User = id;
            tmp.Text = plain;
            tmp.Time = nowTime;
            tmp.Chk = true;
            ((App)Application.Current).SendData("<MSG>", msg);
            ((App)Application.Current).AddSQLChat(target, tmp);

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