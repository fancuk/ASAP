using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TelerikWpfApp3.M;


namespace TelerikWpfApp3.VM
{
    class StartWindowViewModel : INotifyPropertyChanged
    {
        private UserControlViewModel _viewModel1;
        private UserControlViewModel1 _viewModel2;
        private ChatUserControlViewModel _chatViewModel;
        private string _myId;

        public string myId
        {
            get
            {
                return this._myId;
            }
            set
            {
                this._myId = value;
                OnPropertyChanged("myId");
            }
        }
        public ICommand Page1 { get; set; }
        public ICommand Page2 { get; set; }
        public ICommand ChatPageOn { get; set; }
        public ICommand CloseCommand { get; set; }

        public ICommand LogOut { get; set; }

        public StartWindowViewModel()
        {
            this._viewModel1 = new UserControlViewModel();
            this._viewModel2 = new UserControlViewModel1();
            this._chatViewModel = new ChatUserControlViewModel();
            Page1 = new Command(Page1Load, CE);
            Page2 = new Command(Page2Load, CE);
            ChatPageOn = new Command(loadChatPage, CE);
            ContentView = null;
            //CloseCommand = new Command(ExecuteClose, CE);
            myId = ((App)Application.Current).myID;
            LogOut = new Command(logout, CE);
        }
        public void logout(object obj)
        {
            if (MessageBox.Show("로그아웃 하시겠습니까?",
                "로그아웃", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (((App)Application.Current).nowConnect == true)
                {
                    ((App)Application.Current).CloseSocket();
                }
                Window vt = TelerikWpfApp3.viewtest.Instance;
                Window sw = TelerikWpfApp3.StartWindow.Instance;
                vt.Show();
                sw.Hide();
            }
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Handle closing logic, set e.Cancel as needed
            e.Cancel = true;
            if (((App)Application.Current).nowConnect == true)
            {
                ((App)Application.Current).CloseSocket();
            }
            Window vt = TelerikWpfApp3.viewtest.Instance;
            Window sw = TelerikWpfApp3.StartWindow.Instance;
            vt.Show();
            sw.Hide();
        }
        private bool CE(object obj)
        {
            return true;
        }
        private object _contentView;
        private void loadChatPage(object obj)
        {
            this.ContentView = this._chatViewModel;
        }

        private void Page1Load(object obj)
        {
              this.ContentView = this._viewModel1;
        }
        private void Page2Load(object obj)
        {
            this.ContentView = this._viewModel2;
        }
        public object ContentView
        {
            get { return this._contentView; }
            set
            {
                this._contentView = value;
                this.OnPropertyChanged("ContentView");
            }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
