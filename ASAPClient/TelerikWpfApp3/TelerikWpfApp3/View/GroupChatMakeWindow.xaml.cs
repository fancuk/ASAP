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
        public GroupChatMakeWindow()
        {
            InitializeComponent();
            selectedCollection = new List<FriendsItem>();
            ClientList.DataContext = FriendsUserControlViewModel.Instance.getFriends();

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
        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            List<FriendsItem> selcon = new List<FriendsItem>();
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

    }
}
