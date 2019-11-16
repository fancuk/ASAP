using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TelerikWpfApp3.VM;
using TelerikWpfApp3.M;

namespace TelerikWpfApp3.View.UserControl
{
    /// <summary>
    /// ChatUserControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChatUserControl
    {


        private void OnPropertyChanged(string v)
        {
            throw new NotImplementedException();
        }

        public void initThis()
        {

        }
        public ChatUserControl()
        {
            InitializeComponent();
           // ((App)Application.Current).mqState = true;
            //((App)Application.Current).LoadMyFriends();
            //ClientList.DataContext = ((App)Application.Current).getFriends(); 다민
            ChatRoomList.DataContext = FriendsUserControlViewModel.Instance.getFriends();
            //this.PreviewKeyDown += new KeyEventHandler(OnEnterKeyDownHandler);


        }
        private void GetMessageById(object sender, RoutedEventArgs e)
        {
            string target = (((sender as StackPanel).FindName("TargetBox") as TextBlock).Text);
            //chatTarget.Text = target;
            // ChatBox.DataContext = ((App)Application.Current).getChat(target);
            //((App)Application.Current).nowChatTarget = (target);
            refresh();

            //UpdateScrollBar(ChatBox);
        }

        private void refresh()
        {
        }

        // 친구 클릭시 아래로 내려감.
        private void UpdateScrollBar(ListBox listBox)
        {
            if (listBox != null)
            {
                var border = (Border)VisualTreeHelper.GetChild(listBox, 0);
                var scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }

        }

        // 여기는 메세지 박스내에서 엔터시 focus 없애는것.

        /* private void OnEnterKeyDownHandler(object sender, KeyEventArgs e)
         {
             if (e.Key == Key.Return)
             {
                 if (MessageBox.IsFocused)
                 {
                     HyperlinkAutomationPeer peer = new HyperlinkAutomationPeer(sendTextMsgButton);
                     IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                     invokeProv.Invoke();
                     MessageBox.Focus();
                 }
                 else if (FriendNameInput.IsFocused)
                 {
                     HyperlinkAutomationPeer peer = new HyperlinkAutomationPeer(FriendAdd);
                     IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                     invokeProv.Invoke();
                 }
                 else
                 {
                     MessageBox.Focus();
                 }
             }

         }*/
        private void RoomDoubleClick(object sender, RoutedEventArgs e)
        {
            string target = null;
            foreach (FriendsItem obj in ChatRoomList.SelectedItems)
            {
                target = obj.User.ToString();
                //string myId = ((App)Application.Current).myID; 
                //((App)Application.Current).SendData("<CHR>", myId + "/target"); <CHR> 태그 추가
            }
            /*Window ChatRooms = new ChatRoom(target);
            ChatRooms.Show();다민*/
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
        // 새로운 대화가 추가될때 선택되는 형태이므로 선택에 변화가 생기면 Unselect 해주게 변경.
        private void ChatBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}