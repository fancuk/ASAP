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
    class StartWindowViewModel: INotifyPropertyChanged
    {
        private UserControlViewModel _viewModel1;
        private UserControlViewModel1 _viewModel2;
        private ChatUserControlViewModel _chatViewModel;
        public ICommand Page1 { get; set; }
        public ICommand Page2 { get; set; }
        public ICommand ChatPageOn { get; set; }
        public ICommand CloseCommand { get; set; }


        public StartWindowViewModel()
        {
            this._viewModel1 = new UserControlViewModel();
            this._viewModel2 = new UserControlViewModel1();
            this._chatViewModel = new ChatUserControlViewModel();
            Page1 = new Command(Page1Load, CE);
            Page2 = new Command(Page2Load, CE);
            ChatPageOn = new Command(loadChatPage, CE);
            ContentView = null;
            CloseCommand = new Command(ExecuteClose, CE);

        }
        private void ExecuteClose(object obj)
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
