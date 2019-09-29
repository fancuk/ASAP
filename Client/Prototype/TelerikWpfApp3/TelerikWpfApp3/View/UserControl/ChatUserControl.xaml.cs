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

namespace TelerikWpfApp3.View.UserControl
{
    /// <summary>
    /// ChatUserControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChatUserControl 
    {
        public void initThis()
        {

        }
        public ChatUserControl()
        {
            InitializeComponent();
            ((App)Application.Current).mqState = true;
            ((App)Application.Current).loadAllChat();
            //((App)Application.Current).LoadMyFriends();
            sendMsgStackPanel.Height = 0;
            defaultContent.Height = 540;
            ClientList.DataContext = ((App)Application.Current).getFriends();
            this.PreviewKeyDown += new KeyEventHandler(OnEnterKeyDownHandler);
        }
        private void GetMessageById(object sender, RoutedEventArgs e)
        {
            defaultContent.Height = 0;
            sendMsgStackPanel.Height = 80;
            string target  = (((sender as StackPanel).FindName("TargetBox") as TextBlock).Text);
            chatTarget.Text = target;
            ChatBox.DataContext = ((App)Application.Current).getChat(target);
            ((App)Application.Current).nowChatTarget = (target);
            refresh();

            UpdateScrollBar(ChatBox);
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

        private void OnEnterKeyDownHandler(object sender, KeyEventArgs e)
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

        }

    }
}
