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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace TelerikWpfApp3.View.UserControl
{
    /// <summary>
    /// FriendUserControl.xaml에 대한 상호 작용 논리
    /// </summary>s
    public partial class FriendUserControl
    {
        public FriendUserControl()
        {
            InitializeComponent();
            ((App)Application.Current).mqState = true;
            ((App)Application.Current).loadAllChat();
            //((App)Application.Current).LoadMyFriends();
            //sendMsgStackPanel.Height = 0;
            //defaultContent.Height = 540;
            ClientList.DataContext = ((App)Application.Current).getFriends();
            //this.PreviewKeyDown += new KeyEventHandler(OnEnterKeyDownHandler);
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
            Window ChatRooms = new ChatRoom(target);
            ChatRooms.Show();
        }

        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
