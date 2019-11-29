using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using TelerikWpfApp3.VM;
using TelerikWpfApp3.Service;

namespace TelerikWpfApp3.View
{
    /// <summary>
    /// GroupChatRoom.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GroupChatRoom : INotifyPropertyChanged
    {
        private string groupidx;
        GroupChatManager gcm = ((App)Application.Current).groupChatManager;
        GroupChatRoomViewModel gc = new GroupChatRoomViewModel();

        public GroupChatRoom(string groupidx)
        {
            InitializeComponent();
            this.MouseLeftButtonDown += MoveWindow;
            gc.gIdx = groupidx;
            gc.groupChatName = gcm.getGroupName(groupidx);
            this.DataContext = gc;

            Closing += gc.OnWindowClosing;
            ChatBox.DataContext = gcm.loadChat(gc.gIdx);
            UpdateScrollBar(ChatBox);
            this.PreviewKeyDown += new KeyEventHandler(OnEnterKeyDownHandler);
           
        }
        /*public void MakeChatRoom(string target) //다민
        {
            setTarget(target);
            ChatUserControlViewModel cu = new ChatUserControlViewModel();
            cu.target = target;
            this.DataContext = cu;
            ChatBox.DataContext = ((App)Application.Current).getChat(this.target);
            ((App)Application.Current).nowChatTarget = (this.target);
            UpdateScrollBar(ChatBox);
            this.PreviewKeyDown += new KeyEventHandler(OnEnterKeyDownHandler);
        }*/
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
        void MoveWindow(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }

        public void setGroupIndex(string groupidx)
        {
            this.groupidx = groupidx;
        }
        private void UpdateScrollBar(ListBox listBox)
        {
            //if (listBox.Items.Count == 0)
            //{
            //    var border = (Border)VisualTreeHelper.GetChild(listBox, 0);
            //    var scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
            //    scrollViewer.ScrollToBottom();
            //}
        }
        private void ChatBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChatBox.UnselectAll();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

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
                else
                {
                    MessageBox.Focus();
                }
            }

        }

        private void ChattingRoom_Activated(object sender, EventArgs e) // Focus 상태
        {
            
        }

        private void ChattingRoom_Deactivated(object sender, EventArgs e) // LostFocus 상태
        {
           
        }
    }
}
