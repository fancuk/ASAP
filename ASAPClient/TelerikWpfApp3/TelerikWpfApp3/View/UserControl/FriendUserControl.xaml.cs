using System;
using System.Collections.Generic;
using System.Linq;
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
using TelerikWpfApp3.Service;
using TelerikWpfApp3.VM;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace TelerikWpfApp3.View.UserControl
{
    /// <summary>
    /// FriendUserControl.xaml에 대한 상호 작용 논리
    /// </summary>s
    public partial class FriendUserControl
    {
        ChatManager chatManager = ((App)Application.Current).chatManager;
        public FriendUserControl()
        {
            InitializeComponent();
            this.DataContext = FriendsUserControlViewModel.Instance;
           // ((App)Application.Current).mqState = true;
           // ((App)Application.Current).loadAllChat();
            //((App)Application.Current).LoadMyFriends();
            //sendMsgStackPanel.Height = 0;
            //defaultContent.Height = 540;
            //ClientList.DataContext = ((App)Application.Current).getFriends(); 다민
            ClientList.DataContext = FriendsUserControlViewModel.Instance.getFriends(); 
            //this.PreviewKeyDown += new KeyEventHandler(OnEnterKeyDownHandler);
        }

        private void txtNameToSearch_TextChanged(object sender,
TextChangedEventArgs e)
        {
            string txtOrig = txtNameToSearch.Text;
            string upper = txtOrig.ToUpper();
            string lower = txtOrig.ToLower();

            var empFiltered = from Emp in FriendsUserControlViewModel.Instance.getFriends()
                              let ename = Emp.User
                              where
                               ename.StartsWith(lower)
                               || ename.StartsWith(upper)
                               || ename.Contains(txtOrig)
                              select Emp;

            ClientList.DataContext = empFiltered;
        }

        private void GetMessageById(object sender, RoutedEventArgs e)
        {
          
        }
        private void FriendDoubleClick(object sender, RoutedEventArgs e)
        {
            string target = null;
            foreach(FriendsItem obj in ClientList.SelectedItems)
            {
                target = obj.User.ToString();
            }
            chatManager.myRead(target);
            if (ChattingRoomManager.Instance.findChatRoom(target)) //다민
            {
                ChattingRoomManager.Instance.makeChatRoom(target);
                ChattingRoomManager.Instance.showChatRoom(target);
            }
            else
            {
                ChattingRoomManager.Instance.showChatRoom(target);
            }
        }

        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
