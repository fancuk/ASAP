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
        object a;
        public ChatUserControl()
        {
            InitializeComponent(); 
            object b = ((App)Application.Current).getClient();
            ClientList.DataContext = b;
            GetMessageById2();
        }
        private void GetMessageById(object sender, RoutedEventArgs e)
        {
            string target  = (((sender as StackPanel).FindName("TargetBox") as TextBlock).Text);
            a = ((App)Application.Current).getChat(target);
        }
        private void GetMessageById2()
        {
            string target = "12";
            // string target  = (((sender as StackPanel).FindName("TargetBox") as TextBlock).Text);
            a = ((App)Application.Current).getChat(target);
            ChatBox.DataContext = a;

            target = "12";


        }
        private void ChatBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            ChatBox.UpdateLayout();
            ChatBox.ScrollIntoView(ChatBox.Items[ChatBox.Items.Count - 1]);
        }
        
        private void ChatBox_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            ChatBox.UpdateLayout();
            ChatBox.ScrollIntoView(ChatBox.Items[ChatBox.Items.Count - 1]);
        }
    }
}
