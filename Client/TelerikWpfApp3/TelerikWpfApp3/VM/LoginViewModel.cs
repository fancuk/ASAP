using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using TelerikWpfApp3.M;

namespace TelerikWpfApp3.VM
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private string uid;
        private string upw;

        public string Uid
        {
            get { return this.uid; }
            set
            {
                this.uid = value;
                OnPropertyChanged("Uid");
            }
        }
        public string Upw
        {
            get { return this.upw; }
            set
            {
                this.upw = value;
                OnPropertyChanged("Upw");
            }
        }
        public string InputBoxColor { get; set; }

        public ICommand CloseCommand { get; set; }
        public ICommand test { get; set; }
        public ICommand Register { get; set; }
        public ICommand textChange { get; set; }
        public ICommand Func1 { get; set; }
        public ICommand login { get; set; }
        public LoginViewModel()
        {
            BoxColor = new SolidColorBrush(Colors.Gray);
            Register = new Command(ExecuteGotoRegister, CanExecute);
            //login = new Command(ExecuteLogin, CanExecute);
            textChange = new Command(ExecuteTextChange, CanExecute);
            test = new Command(ExecuteTest, CanExecute);
            CloseCommand = new Command(ExecuteClose, CanExecute);
        }
        private void ExecuteClose(object obj)
        {
            if (((App)Application.Current).nowConnect == true)
            {
                ((App)Application.Current).CloseSocket();
            }
        }


        private void ExecuteTest(object obj)
        {
            MessageBox.Show("Test 중인 기능입니다.");
        }
        private void ExecuteGotoRegister(object obj)
        {
            Window s = TelerikWpfApp3.Register.Instance;
            s.Owner = Application.Current.MainWindow; // We must also set the owner for this to work.

           // s.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            s.Top = Application.Current.MainWindow.Top;
            s.Left = Application.Current.MainWindow.Left+349;
            s.Show();
        }

        private void ExecuteTextChange(object obj)
        {


        }

        public bool ChangeColor { get; set; }
        public Brush BoxColor
        {
            get;set;
        }

        private bool CanExecute(object obj)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
