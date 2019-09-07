using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
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

namespace TelerikWpfApp3
{
    /// <summary>
    /// Register.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Register : Window
    {
        public class pwChk:INotifyPropertyChanged
        {
            private string _ckhreuslt;
            public string chkResult
            {
                get { return this._ckhreuslt; }
                set
                {
                    this._ckhreuslt = value;
                    OnPropertyChanged("chkResult");
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
        pwChk pc = new pwChk();
        public Register()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += MoveWindow;
            pwchk.DataContext = pc;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
        void MoveWindow(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }

        private void Pwbox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string p1 = pwbox.Password.ToString();
            string p2 = pwbox2.Password.ToString();
            if (p1.Equals(p2))
            {
                pc.chkResult = "pwEqual";
                pwchk.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                pc.chkResult = "not Equal";
                pwchk.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void Pwbox2_LostFocus(object sender, RoutedEventArgs e)
        {
            string p1 = pwbox.Password.ToString();
            string p2 = pwbox2.Password.ToString();
            if (p1.Equals(p2))
            {
                pc.chkResult = "pwEqual";
                pwchk.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                pc.chkResult = "not Equal";
                pwchk.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
