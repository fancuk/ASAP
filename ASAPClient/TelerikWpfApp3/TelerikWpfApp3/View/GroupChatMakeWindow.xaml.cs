using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TelerikWpfApp3.Collection;
using TelerikWpfApp3.M;
using TelerikWpfApp3.Service;
using TelerikWpfApp3.VM;

namespace TelerikWpfApp3.View
{
    /// <summary>
    /// GroupChatMakeWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GroupChatMakeWindow : Window
    {
        ChatManager chatManager = ((App)Application.Current).chatManager;
        ObservableCollection<string> addedCollection = new ObservableCollection<string>();
        List<FriendsItem> selectedCollection;
        private bool status = false;
        List<FriendsItem> selcon;

        public GroupChatMakeWindow()
        {
            InitializeComponent();
            selectedCollection = new List<FriendsItem>();
            ClientList.DataContext = FriendsUserControlViewModel.Instance.getFriends();
            selcon = new List<FriendsItem>();
        }
  
        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            selcon = new List<FriendsItem>(); //선택된 친구들
            int len = lb.SelectedItems.Count;
            for(int i=0; i<len; i++)
            {
                FriendsItem fi = (FriendsItem)lb.SelectedItems[i];
                selcon.Add(fi);
            }
            addedList.DataContext = selcon;
        }
        private void FriendDoubleClick(object sender, RoutedEventArgs e)
        {
          
        }
     
        private void radioCheck(object sender, RoutedEventArgs e)
        {
            ToggleButton rb = (ToggleButton)sender;
            if (rb.IsChecked == true)
            {
                //rb.ance
                // 추가
                addedList.Items.Add(rb.Content);
            }
            else
            {
                // 삭제
                addedList.Items.Remove(rb.Content);
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            string groupName = groupNameTxt.Text as string;
            if (groupName == "")
            {
                MessageBox.Show("그룹 이름을 입력 하세요");
                return;
            }
            int len = selcon.Count;
            if (len < 2)
            {
                MessageBox.Show("2명 이상 선택하세요");
                return;
            }
            string groupMembers = "";
            for (int i = 0; i < len; i++)
            {
                groupMembers += selcon[i].User as string;
                groupMembers += "^";

            }
            len += 1; //나 자신 넣어야함
            string groupMemberCount = len.ToString();
            // parameter 가 선택된 친구들의 id를 담고 있는 string List임 
            // module을 생성하여 바인딩 하면 된다.
            // groupName은 그룹의 이름을 가지고 있는 string 임
            GroupChatMakeWindowViewModel groupChatMakeWindowVM = new GroupChatMakeWindowViewModel();
            groupChatMakeWindowVM.makeGroupChat(groupName, groupMembers, groupMemberCount);
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
