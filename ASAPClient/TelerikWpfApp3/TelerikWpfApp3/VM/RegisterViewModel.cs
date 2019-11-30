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
using TelerikWpfApp3.Service;

namespace TelerikWpfApp3.VM
{
    class RegisterViewModel : INotifyPropertyChanged
    {
        NetworkManager networkManager = ((App)Application.Current).networkManager;

        #region type
        private string _pw1;
        private string _pw2;
        private string _pw1Chk;
        private string _pwChk;
        private string _name;
        private string _nameChk;
        private string _email;
        private string _emailChk;
        private string _nowSelectedEmail;
        #endregion


        #region properties

        public string name
        {
            get { return this._name; }
            set
            {
                this._name = value; OnPropertyChanged("name");
                ((App)Application.Current).userStatusManager.Idchk = false;
                if (this._name != "")
                {
                    nameChk = "X";
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
            set
            {
                this._email = value;
                OnPropertyChanged("email");
                if (this._nowSelectedEmail != null)
                {
                    emailChk = "V";
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
            set
            {
                this._pw1 = value; OnPropertyChanged("pw1");
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
        } */

        public string pwChk
        {
            get { return this._pwChk; }
            set { this._pwChk = value; OnPropertyChanged("pwChk"); }
        }

        public string NowSelectedEmail
        {
            get => _nowSelectedEmail;
            set
            {
                _nowSelectedEmail = value;
                OnPropertyChanged("NowSelectedEmail");
                if (this._email != null)
                {
                    emailChk = "V";
                }
            }
        }

        public ObservableCollection<string> emailList { get; set; }
        #endregion

        #region methods
        public ICommand pw2Changed { get; set; }
        public ICommand test { get; set; }
        public ICommand idChecking { get; set; }
        public ICommand emailSelecting { get; set; }
        public ICommand CloseCommand { get; set; }
        #endregion

        public RegisterViewModel()
        {
            nameChk = "X";
            emailChk = "X";
            CloseCommand = new Command(ExecuteClose, CanExecute);
            idChecking = new Command(idCheckButton, CanExecute);
            emailList = new ObservableCollection<string>()
            {
                 "@naver.com","@gamil.com","@daum.net"
            };
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Handle closing logic, set e.Cancel as needed
            e.Cancel = true;
            if (networkManager.nowConnect == true)
            {
                networkManager.CloseSocket();
            }

            Window rv = TelerikWpfApp3.Register.Instance;
            Register rr = (Register)rv;
            rr.allReset();
            rv = rr;
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
                if (networkManager.StartSocket() == true)
                {
                    networkManager.SendData("<ICF>", id);
                    networkManager.ReceiveSocket();
                }
            }
        }

        public void ExecuteRegister(MyInfo myinfo)
        {
            string parameter = myinfo.MyId + "/" + myinfo.Pw + "/" + myinfo.Email + "/";
            networkManager.SendData("<REG>",parameter);
            networkManager.ReceiveSocket();
        }

        private void ExecuteClose(object obj)
        {
            if (networkManager.nowConnect == true)
            {
                networkManager.CloseSocket();
            }
        }

        private bool CanSelect(object msg)
        {
            if ((string)msg == "")
            {
                return false;
            }
            return true;
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
