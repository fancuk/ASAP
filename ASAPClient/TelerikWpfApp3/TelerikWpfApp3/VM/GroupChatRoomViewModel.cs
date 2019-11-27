using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TelerikWpfApp3.M;
using TelerikWpfApp3.Service;
using TelerikWpfApp3.Utility;

namespace TelerikWpfApp3.VM
{
    class GroupChatRoomViewModel : INotifyPropertyChanged
    {
        DabbingPreventor dabbingPreventor = ((App)Application.Current).dabbingPreventor;
        NetworkManager networkManager = ((App)Application.Current).networkManager;

        public ICommand GroupSendText { get; set; } // 그룹 채팅방 전송 버튼
        public GroupChatRoomViewModel()
        {
            GroupSendText = new Command(ExecuteGroupSendText, CanExecuteMethod);
        }
        public void ExecuteGroupSendText(object org)
        {
            if (string.IsNullOrWhiteSpace(org as string) == true)
            {
                MessageBox.Show("메세지를 입력해주세요.");
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
        public void sendMessage(string tag, string message)
        {
            string id = networkManager.MyId;
            string plain = message;
            string nowTime = DateTime.Now.ToString();
            // 여기다가 기능 추가!!!!
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
