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
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace TelerikWpfApp3.VM
{
    class RegisterViewModel : INotifyPropertyChanged
    {
        #region type
        private string _pw1;
        private string _pw2;
        private string _pw1Chk;
        private string _pwChk;
        private string _name;
        private string _nameChk;
        private string _email;
        private string _emailChk;
        #endregion


        #region properties..

        public string name
        {
            get { return this._name; }
            set
            {
                this._name = value; OnPropertyChanged("name");
                ((App)Application.Current).idchk = false;
                if (this._name != "")
                {
                }
                else nameChk = "X";
                /*if (((App)Application.Current).getidchk() == true)
               {
                   nameChk = "V";
               }
               else
               {
                   nameChk = "X";
               }*/

            }
        }
        public string nameChk
        {
            get { return this._nameChk; }
            set { this._nameChk = value; OnPropertyChanged("nameChk"); }
        }
        public string email
        {
            get { return this._email; }
            set { this._email = value; OnPropertyChanged("email");
                if (Regex.IsMatch(this._email, @"^[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                {
                    emailChk = "V";
                    ((App)Application.Current).emailChk = (true);
                }
                else
                {
                    emailChk = "X";
                    ((App)Application.Current).emailChk = (false);
                }
            }
        }
        public string emailChk
        {
            get { return this._emailChk; }
            set { this._emailChk = value; OnPropertyChanged("emailChk"); }
        }
       public string pw1
        {
            get { return this._pw1; }
            set { this._pw1 = value; OnPropertyChanged("pw1");
               /* if (this._pw1 != "")
                {
                    pw1Chk = "V";
                }
                else pw1Chk = "X";*/
            }
        }
        
        public string pw1Chk
        {
            get { return this._pw1Chk; }
            set
            {
                this._pw1Chk = value; OnPropertyChanged("pw1Chk");
            }
        }
        /*
        public string pw2
        {
            get { return this._pw2; }
            set { this._pw2 = value; OnPropertyChanged("pw2"); pwCheck(); }
        }

            */
        public string pwChk
        {
            get { return this._pwChk; }
            set { this._pwChk = value; OnPropertyChanged("pwChk"); }
        }
        
        #endregion

        #region methods
        public ICommand pw2Changed { get; set; }
        public ICommand test { get; set; }
        public ICommand idChecking { get; set; }

        #endregion

        public ICommand CloseCommand { get; set; }

        public RegisterViewModel()
        {
            nameChk = "X";
            emailChk = "X";
            //pw2Changed = new Command(ExecuteChkPwEquals, CanExecute);
            CloseCommand = new Command(ExecuteClose, CanExecute);
            idChecking = new Command(idCheckButton, CanExecute);
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Handle closing logic, set e.Cancel as needed
            e.Cancel = true;
            if (((App)Application.Current).nowConnect == true)
            {
                ((App)Application.Current).CloseSocket();
            }
            Window rv = TelerikWpfApp3.Register.Instance;
            rv.Hide();
        }
        private void idCheckButton(object org)
        {
            string id = name;
            if (id == null)
            {
                MessageBox.Show("ID를 입력해주세요.");
            }
            else if (!Regex.IsMatch(id, @"^[a-z0-9]{5,10}$"))
            {
                MessageBox.Show("5~10자리 숫자, 영문 소문자만 가능합니다.");
            }
            else
            {
                ((App)Application.Current).StartSocket();
                ((App)Application.Current).SendData("<ICF>", id);
            }
        }

        private void ExecuteClose(object obj)
        {
            if (((App)Application.Current).nowConnect == true)
            {
                ((App)Application.Current).CloseSocket();
            }
        }

        /*private void ExecuteChkPwEquals(object obj)
        {
            MessageBox.Show("ss");
            if (this.pw1.Equals(this.pw2))
            {
  
            }
        }*/

        private bool CanExecute(object obj)
        {
            return true;
        }

        /*private void pwCheck()
        {
            if (pw1.Equals(pw2))
            {
                pwChk = "PassWord Equals!";
            }
            else
            {
                pwChk = "Reconfirm your Password!!!";
            }
        }*/

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
