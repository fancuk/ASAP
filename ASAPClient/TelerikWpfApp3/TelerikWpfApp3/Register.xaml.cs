using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
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
using TelerikWpfApp3.VM;
using TelerikWpfApp3.Service;

namespace TelerikWpfApp3
{
    /// <summary>
    /// Register.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Register : Window
    {
        RegisterViewModel rvm;

        public class pwChk : INotifyPropertyChanged
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

        public class pw1Chk : INotifyPropertyChanged
        {
            private string _passChk;
            public string passChk
            {
                get { return this._passChk; }
                set
                {
                    this._passChk = value;
                    OnPropertyChanged("passChk");
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        pwChk pc = new pwChk();
        pw1Chk pwd = new pw1Chk();

        private static Register instance = null;

        public static Register Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Register();
                }
                return instance;
            }
        }

        private Register()
        {
            InitializeComponent();
            pwd.passChk = "X";
            pw1chk.Foreground = new SolidColorBrush(Colors.Red);
            this.MouseLeftButtonDown += MoveWindow;
            pwchk.DataContext = pc;
            pw1chk.DataContext = pwd;
            rvm = new RegisterViewModel();
            this.DataContext = rvm;
            Closing += rvm.OnWindowClosing;
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

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string id = idbox1.Text;
            string pw1 = pwbox.Password.ToString();
            string email = emailBox.Text;
            
            string parameter = id + "/" + pw1 + "/" + email + "/";
            
            bool isit = ((App)Application.Current).userStatusManager.Idchk;
            if (!isit) //id chk 안함
            {
                MessageBox.Show("아이디 체크 해주세요.");
            }
            else if (id == "" || pw1 == "" || email == "")
            {
                MessageBox.Show("아이디, 비밀번호, 이메일은 공백일 수 없습니다.");
            }
            else if (!Regex.IsMatch(pw1, @"^[a-z0-9]{8,15}$"))
            {
                MessageBox.Show("비밀번호는 8~15자리 숫자, 영문 소문자만 가능합니다.");
            }
            else if (pc.chkResult == "not Equal")
            {
                MessageBox.Show("비밀번호를 다시 확인해주세요.");
            }
            else if (emailSelect.SelectedItem == null)
            {
                MessageBox.Show("이메일을 선택해주세요");
            }
            else if (agree.IsChecked == false)
            {
                MessageBox.Show("동의 버튼을 눌러주세요.");
            }
            else
            {
                rvm.ExecuteRegister(new MyInfo(id, pw1, email));
            }
        }

        private void Pwbox2_TextChanged(object sender, TextChangedEventArgs e) 
        {
            string p1 = pwbox.Password.ToString();
            string p2 = pwbox2.Password.ToString();
            if (p1.Equals(p2) && Regex.IsMatch(p1, @"^[a-z0-9]{8,15}$"))
            {
                pc.chkResult = "pwEqual";
                pwchk.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                pc.chkResult = "not Equal";
                pwchk.Foreground = new SolidColorBrush(Colors.LightGray);
            } 
        }

        private void Pwbox2_LostFocus(object sender, RoutedEventArgs e)
        {
            string p1 = pwbox.Password.ToString();
            string p2 = pwbox2.Password.ToString();
            if (p1.Equals(p2) && Regex.IsMatch(p1, @"^[a-z0-9]{8,15}$"))
            {
                pc.chkResult = "pwEqual";
                pwchk.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                pc.chkResult = "not Equal";
                pwchk.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void Pwbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string p1 = pwbox.Password.ToString();

            if (Regex.IsMatch(p1, @"^[a-z0-9]{8,15}$") && p1 != "")
            {
                pwd.passChk = "V";
                pw1chk.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                pwd.passChk = "X";
                pw1chk.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void Pwbox_LostFocus(object sender, RoutedEventArgs e)
        {
            string p1 = pwbox.Password.ToString();

            if (Regex.IsMatch(p1, @"^[a-z0-9]{8,15}$") && p1 != "")
            {
                pwd.passChk = "V";
                pw1chk.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                pwd.passChk = "X";
                pw1chk.Foreground = new SolidColorBrush(Colors.LightGray);
                MessageBox.Show("비밀번호는 8~15자리 숫자, 영문 소문자만 가능합니다.");
            }
        }

        public void allReset()
        {
            pw1chk.Text = "";
            idbox1.Text = "";
            pwbox2.Text = "";
            emailBox.Text = "";
            agree.IsChecked = false;
        }
    }
}
