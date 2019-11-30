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
using System.Windows.Shapes;
using TelerikWpfApp3.VM;
using TelerikWpfApp3.Service;

namespace TelerikWpfApp3.View
{
    /// <summary>
    /// GroupFriendShow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GroupFriendShow : Window
    {
      
        public GroupFriendShow(string gidx)
        {
            InitializeComponent();
            GroupChatRoomViewModel groupChatRoomViewModel = new GroupChatRoomViewModel();
            ClientList.DataContext = groupChatRoomViewModel.getGroupMemberList(gidx);
            
        }
    }
}
