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
using TelerikWpfApp3.Utility;

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
                return this._target;
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
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Handle closing logic, set e.Cancel as needed
            e.Cancel = true;
            ChattingRoomManager.Instance.closeChatRoom(target);
        }
        public ICommand SendText { get; set; }
        public ICommand friendPlus { get; set; }
        public ChatUserControlViewModel()
        {
            //friendPlus = new Command(fpButton, CanExecuteMethod);
            SendText = new Command(ExeceuteSendMsg, CanExecuteMethod);
        }

       /* public void fpButton(object org)
        {
            //친구추가 공백 방지!  
            if (string.IsNullOrWhiteSpace(org as string) == true)
            {
                MessageBox.Show("추가할 친구를 입력해주세요.");
            }
            else
            {//친구추가 중복시 방지!
                if (FriendsUserControlViewModel.Instance.FriendDoubleCheck(org as string)
                    //((App)Application.Current).FriendDoubleCheck(org as string)다민)
                {
                    MessageBox.Show("해당 친구는 친구목록에 존재합니다.");
                }
                else
                {
                    searchName = org as string;
                    string str = searchName + "/";
                    string member = ((App)Application.Current).myID;
                    ((App)Application.Current).SendData("<FRR>", str + member);
                }

            }
        }*/

        public void ExeceuteSendMsg(object org)
        {//메세지 공백 방지!
            if (string.IsNullOrWhiteSpace(org as string) == true)
            {
                MessageBox.Show("메세지를 입력해주세요.");
            }
            else
            {
                DabbingPreventor dabbingPreventor = DabbingPreventor.Instance;

                if (dabbingPreventor.isDabbing())
                {
                    MessageBox.Show("도배 하지 마세요!");
                    return;
                }
                string id = ((App)Application.Current).myID;
                target = ((App)Application.Current).nowChatTarget;
                string plain = org as string;
                string nowTime = DateTime.Now.ToString();
                string msg = target + "/" + id + "/" + nowTime + "/" + plain + "/";
                sqlite.ChattingCreate(id, target, nowTime, plain, "Send");
                Chatitem tmp = new Chatitem();
                tmp.User = id;
                tmp.Text = plain;
                tmp.Time = nowTime;
                tmp.Chk = true;
                ((App)Application.Current).SendData("<MSG>", msg);
                ((App)Application.Current).AddSQLChat(target, tmp);
                
                msgTextBox = "";
            }

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