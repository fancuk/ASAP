using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using System.ComponentModel;

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

        public ICommand Register { get; set; }
        public LoginViewModel()
        {
            BoxColor = new SolidColorBrush(Colors.Gray);
            Register = new Command(ExecuteGotoRegister, CanExecute);
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Handle closing logic, set e.Cancel as needed
            e.Cancel = true;
            if (((App)Application.Current).nowConnect == true)
            {
                ((App)Application.Current).CloseSocket();
            }
            Process.GetCurrentProcess().Kill();
        }

        private void ExecuteGotoRegister(object obj)
        {
            Window s = TelerikWpfApp3.Register.Instance;
            s.Owner = Application.Current.MainWindow; // We must also set the owner for this to work.

           // s.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            s.Top = Application.Current.MainWindow.Top;
            s.Left = Application.Current.MainWindow.Left+395;
            s.Show();
        }

        public void LogIn(string id,string pw)
        {
            ((App)Application.Current).setmyID(Uid);
            string parameter = Uid + "/" + Upw;
            ((App)Application.Current).StartSocket();
            if (((App)Application.Current).nowConnect == true)
            {
                ((App)Application.Current).SendData("<LOG>", parameter);
                ((App)Application.Current).setmyID(Uid);
            }
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
