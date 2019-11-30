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
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Threading;

namespace TelerikWpfApp3.View
{
    /// <summary>
    /// ChatRoom.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChatRoom : INotifyPropertyChanged
    {
        private string target;
        ChatUserControlViewModel cu = new ChatUserControlViewModel();
        ChatManager cm = ((App)Application.Current).chatManager;
        /*private static ChatRoom instance = null; // 다민

        public static ChatRoom Instance //다민
        {
            get
            {
                if (instance == null)
                {
                    instance = new ChatRoom();
                }
                return instance;
            }
        }*/
        public ChatRoom(string target) 
        {
            InitializeComponent();
            ((Storyboard)FindResource("WaitStoryboard")).Begin();

            this.MouseLeftButtonDown += MoveWindow;
            setTarget(target); 
            cu.target = target; 
            this.DataContext = cu;
   
            ChatBox.DataContext =cu.loadChat(this.target);
            cu.target = (this.target);
            UpdateScrollBar(ChatBox);
            this.PreviewKeyDown += new KeyEventHandler(OnEnterKeyDownHandler);
            Closing += cu.OnWindowClosing;

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

        public void setTarget(string target)
        {
            this.target = target;
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
            runStoryBoard();
        }

        public void runStoryBoard()
        {
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
            bool isit = true;
            cm.AboutFocus(isit, target);
            
        }

        private void ChattingRoom_Deactivated(object sender, EventArgs e) // LostFocus 상태
        {
            bool isit = false;
            cm.AboutFocus(isit, target);
            cm.removeUnReadCount(target);
        }

        private void SendTextMsgButton1_Click(object sender, RoutedEventArgs e)
        {
            AsapTopBar.Height = 68;
            //this.Dispatcher.Invoke((ThreadStart)(() => { }), DispatcherPriority.ApplicationIdle);
            //Thread.Sleep(10000);
            //AsapTopBar.Height = 0;
        }
        public void closeTopBar()
        {
            AsapTopBar.Height = 0;
        }
    }
}
