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
                ((App)Application.Current).setidchk(false);
                if (this._name != "")
                {
                    nameChk = "V";
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
                if (this._email != "")
                {
                    emailChk = "V";
                }
                else emailChk = "X";
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
                if (this._pw1 != "")
                {
                    pw1Chk = "V";
                }
                else pw1Chk = "X";
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
        public string pw2
        {
            get { return this._pw2; }
            set { this._pw2 = value; OnPropertyChanged("pw2"); pwCheck(); }
        }


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
            pw1Chk = "X";
            pw2Changed = new Command(ExecuteChkPwEquals, CanExecute);
            CloseCommand = new Command(ExecuteClose, CanExecute);
            idChecking = new Command(idCheckButton, CanExecute);
        }
        private void idCheckButton(object org)
        {
            string id = name;
            if (id == "")
            {
                MessageBox.Show("ID를 입력해주세요.");
            }
          ((App)Application.Current).StartSocket();
            ((App)Application.Current).SendData("<ICF>", id);
        }

        private void ExecuteClose(object obj)
        {
            if (((App)Application.Current).nowConnect == true)
            {
                ((App)Application.Current).CloseSocket();
            }
        }

        private void ExecuteChkPwEquals(object obj)
        {
            MessageBox.Show("ss");
            if (this.pw1.Equals(this.pw2))
            {
  
            }
        }

        private bool CanExecute(object obj)
        {
            return true;
        }

        private void pwCheck()
        {
            if (pw1.Equals(pw2))
            {
                pwChk = "PassWord Equals!";
            }
            else
            {
                pwChk = "Reconfirm your Password!!!";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
