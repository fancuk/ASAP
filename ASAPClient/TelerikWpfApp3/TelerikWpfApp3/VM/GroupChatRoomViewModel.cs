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
using TelerikWpfApp3.Collection;
using System.Text.RegularExpressions;

namespace TelerikWpfApp3.VM
{
    class GroupChatRoomViewModel : INotifyPropertyChanged
    {
        DabbingPreventor dabbingPreventor = ((App)Application.Current).dabbingPreventor;
        NetworkManager networkManager = ((App)Application.Current).networkManager;
        GroupMemberListManager groupMemberListManager = ((App)Application.Current).groupMemberListManager;
        GroupChatManager groupChatManager = ((App)Application.Current).groupChatManager;
        LocalDAO localDAO = ((App)Application.Current).localDAO;
        private string _msgTextBox;
        public string gIdx; // ChatRoom new 생성자로 계속 만들어주니까 그때 마다 gIdx 넣어주면 될 듯 바인딩 필요 x
        public string _groupChatName;
        public string groupIdx
        {
            get{
                return this.gIdx;
            }
            set
            {
                this.gIdx = value;
                OnPropertyChanged(groupIdx);
            }
        }

        public string groupChatName
        {
            get
            {
                return this._groupChatName;
            }
            set
            {
                this._groupChatName = value;
                OnPropertyChanged(groupChatName);
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
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Handle closing logic, set e.Cancel as needed
            e.Cancel = true;
            GroupChattingRoomManager.Instance.closeChatRoom(gIdx);
        }

        public ICommand GroupSendText { get; set; } // 그룹 채팅방 전송 버튼
        public ICommand GroupFriendListOpen { get; set; }
        public GroupChatRoomViewModel()
        {
            GroupSendText = new Command(ExecuteGroupSendText, CanExecuteMethod);
            GroupFriendListOpen = new Command(ExecuteGroupFriends, CanExecuteMethod);
        }

        public void ExecuteGroupFriends(object obj)
        {
            GroupFriendShow groupFriendShow = new GroupFriendShow(groupIdx);
            groupFriendShow.ShowDialog();
        }
        public void ExecuteGroupSendText(object org)
        {
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
                sendMessage("<GSG>", org as string);
            }
        }
        public ItemsChangeObservableCollection<GroupChatItem> loadChat(string gIdx) // 여기다가 그룹 대화 바인딩 걸면 될 듯
        {
            return groupChatManager.loadChat(gIdx);
        }
        public void sendMessage(string tag, string message)
        {
            string id = networkManager.MyId;
            string plain = message;
            string nowTime = DateTime.Now.ToString();
            string text = id + "/" + gIdx + "/" + plain + "/" + nowTime;
            groupChatManager.addChat(gIdx, new GroupChatItem(plain, id, nowTime, true));
            string groupName = groupChatManager.getGroupName(gIdx);
            groupChatManager.addChattingList(gIdx, groupName, plain, nowTime);
            networkManager.SendData(tag, text); 
            localDAO.GroupChattingCreate(id, gIdx, nowTime, plain);
            msgTextBox = "";
        }
        public List<string> getGroupMemberList(string gIdx) // 여기다가 그룹 사용자들 바인딩 걸면 될 듯
        {
            return groupMemberListManager.getGroupMemberList(gIdx);
        }
        #region INotify 인터페이스
        public event PropertyChangedEventHandler PropertyChanged;
        private bool CanExecuteMethod(object arg)
        {
            return true;
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
