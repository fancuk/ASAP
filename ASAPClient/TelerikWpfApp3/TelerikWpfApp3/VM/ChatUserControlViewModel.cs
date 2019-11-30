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
using TelerikWpfApp3.Utility;
using TelerikWpfApp3.Service;
using TelerikWpfApp3.LocalDB;
using TelerikWpfApp3.View;
using System.Text.RegularExpressions;

namespace TelerikWpfApp3.VM
{
    class ChatUserControlViewModel : INotifyPropertyChanged
    {
        LocalDAO localDAO = new LocalDAO();
        private string _msgTextBox;
        private string searchname;
        private string _target;
        NetworkManager networkManager = ((App)Application.Current).networkManager;
        WindowManager windowManager = ((App)Application.Current).windowManager;
        ChatManager chatManager = ((App)Application.Current).chatManager;
        DabbingPreventor dabbingPreventor = ((App)Application.Current).dabbingPreventor;
        ASAPManager asapManager = ((App)Application.Current).asapManager;
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
        public ICommand MakeGroupEvent { get; set; }
        public ICommand SendText { get; set; }
        public ICommand friendPlus { get; set; }
        public ICommand SendAsap { get; set; }
        public ChatUserControlViewModel()
        {
            MakeGroupEvent = new Command(ExecuteShowMakeGroupWindow, CanExecuteMethod);
            //friendPlus = new Command(fpButton, CanExecuteMethod);
            SendText = new Command(ExecuteSendMsg, CanExecuteMethod);
            SendAsap = new Command(ExecuteSendASAP, CanExecuteMethod);
        }
        public ObservableCollection<Chatitem> loadChat(string target)
        {
            return chatManager.loadChat(target);
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
        public void ExecuteShowMakeGroupWindow(object org)
        {
            GroupChatMakeWindow groupChatMakeWindow = new GroupChatMakeWindow();
            groupChatMakeWindow.ShowDialog();
        }
        //public void ExeceuteSendMsg(object org)
        //{//메세지 공백 방지!
        //    if (string.IsNullOrWhiteSpace(org as string) == true)
        //    {
        //        MessageBox.Show("메세지를 입력해주세요.");
        //    }
        //    else
        //    {
        //        if (dabbingPreventor.isDabbing())
        //        {
        //            MessageBox.Show("도배 하지 마세요!");
        //            return;
        //        }
        //        string id = networkManager.MyId;
        //        string plain = org as string;
        //        string nowTime = DateTime.Now.ToString();
        //        string msg = target + "/" + id + "/" + nowTime + "/" + plain + "/";
        //        string[] tokens = msg.Split('/');
        //        string lastmsg = tokens[3];
        //        Chatitem tmp = new Chatitem();
        //        int isit = chatManager.IsFriendReading(target); //2019-11-22
        //        tmp.User = id;
        //        tmp.Text = plain;
        //        tmp.Time = nowTime;
        //        tmp.Chk = true;
        //        if (isit == 1)
        //        {
        //            tmp.Status = true;
        //        }
        //        else
        //        {
        //            tmp.Status = false;
        //        }
        //        networkManager.SendData("<MSG>", msg);
        //        chatManager.addChat(target, tmp);
        //        msgTextBox = "";
        //        chatManager.addChattingList(target, lastmsg, nowTime);
        //        //localDAO.ChattingCreate(id, target, nowTime, plain, "Send");
        //        localDAO.ChattingCreate(id, target, nowTime, plain, "Send", isit); // 2019-11-22
        //    }

        //}
        // 정구
        // 일반 메세지 Button Command Binding 내용 분할, 아래 sendMessage로 잘라넣었습니다.
        public void ExecuteSendMsg(object org)
        {//메세지 공백 방지!
            if (string.IsNullOrWhiteSpace(org as string) == true)
            {
                MessageBox.Show("메세지를 입력해주세요.");
            }
            else if (Regex.IsMatch((org as string), @"[&^/]"))
            {
                MessageBox.Show("특수문자(^,&,/)는 사용 불가능합니다.");
            }
            else
            {
                if (dabbingPreventor.isDabbing())
                {
                    MessageBox.Show("도배 하지 마세요!");
                    return;
                }
                sendMessage("<MSG>", org as string);
            }
        }

        // 정구
        // ASAP Button Command Binding 용
        public void ExecuteSendASAP(object org) // 서버 업데이트 후에
        {//메세지 공백 방지!
            if (string.IsNullOrWhiteSpace(org as string) == true)
            {
                MessageBox.Show("메세지를 입력해주세요.");
            }
            else if(Regex.IsMatch((org as string), @"[&^/]"))
            {
                MessageBox.Show("특수문자(^,&,/)는 사용 불가능합니다.");
            }
            else
            {
                if (dabbingPreventor.isDabbing())
                {
                    MessageBox.Show("도배 하지 마세요!");
                    return;
                }

                if (asapManager.IsSheLogin(target) != true) // 상대가 로그인인지
                {
                    MessageBox.Show(target + "이 로그 아웃 상태입니다.");
                    return;
                }
                if(asapManager.ASAP_SentCheck(target) != true) // 상대한테 보냈는지
                {
                    MessageBox.Show(target + "한테 이미 보내셨습니다.");
                    return;
                }
                sendMessage("<ASG>", org as string);
            }
        }

        // 정구
        // sendMessage로 server에 보내고, Chat 기록 ---- 이때 ASAP이면 LocalDB에 저장 X, 아니면 저장.
        private void sendMessage(string tag, string message)
        {
            string id = networkManager.MyId;
            string plain = message;
            string nowTime = DateTime.Now.ToString();
            string msg = target + "/" + id + "/" + nowTime + "/" + plain + "/";
            string[] tokens = msg.Split('/');
            string lastmsg = tokens[3];
            Chatitem tmp = new Chatitem();
            int isit = chatManager.IsFriendReading(target); //2019-11-22
            tmp.User = id;
            tmp.Text = plain;
            tmp.Time = nowTime;
            tmp.Chk = true;
            if (isit == 1)
            {
                tmp.Status = true;
            }
            else
            {
                tmp.Status = false;
            }
            networkManager.SendData(tag, msg);
            chatManager.addChat(target, tmp);
            msgTextBox = "";
            chatManager.addChattingList(target, lastmsg, nowTime);
            //localDAO.ChattingCreate(id, target, nowTime, plain, "Send");
            if (tag.Equals("<MSG>"))
                localDAO.ChattingCreate(id, target, nowTime, plain, "Send", isit); // 2019-11-22
            else
            {
                asapManager.ASAP_PlusSentList(target);
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