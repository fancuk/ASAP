﻿using System;
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
        
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string id = idbox1.Text;
            string pw1 = pwbox.Password.ToString();
            string email = emailBox.Text;
            string parameter = id + "/" + pw1 + "/" + email + "/";
            bool isit = ((App)Application.Current).getidchk();
            if (!isit) //id chk 안함
            {
                MessageBox.Show("아이디 체크 해주세요.");
            }
            else if (id == "" || pw1 == "" || email == "")
            {
                MessageBox.Show("아이디, 비밀번호, 이메일은 공백일 수 없습니다.");
            }
            else if (pc.chkResult == "not Equal")
            {
                MessageBox.Show("비밀번호 확인 요망.");
            }
            else if(agree.IsChecked == false)
            {
                MessageBox.Show("동의 버튼을 눌러주세요.");
            }
            else
            {
                ((App)Application.Current).StartSocket();
                ((App)Application.Current).SendData("<REG>", parameter);
            }
        }

       
    }
}
