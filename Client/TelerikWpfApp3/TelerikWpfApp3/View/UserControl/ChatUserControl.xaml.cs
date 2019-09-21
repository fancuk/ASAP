using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TelerikWpfApp3.View.UserControl
{
    /// <summary>
    /// ChatUserControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChatUserControl 
    {
        public ChatUserControl()
        {
            InitializeComponent(); 
            ChatBox.DataContext = ((App)Application.Current).getChat();
            //((App)Application.Current).LoadMyFriends();
            ClientList.DataContext = ((App)Application.Current).getFriends();

        }
        private void GetMessageById(object sender, RoutedEventArgs e)
        {
            string target  = (((sender as StackPanel).FindName("TargetBox") as TextBlock).Text);
            ((App)Application.Current).setChat(target);
            ((App)Application.Current).setTarget(target);
            refresh();
        }

        private void refresh()
        {
            ChatBox.DataContext = ((App)Application.Current).getChat();
            ClientList.DataContext = ((App)Application.Current).getFriends();
        }
    }
}
