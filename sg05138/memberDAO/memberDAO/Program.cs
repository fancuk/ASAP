using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using memberVO;

namespace memberDAO
{
    class Program : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string LogInId = "";
        private string LogInPassword = "";
        private string SignUpId = "";
        private string SignUpPassword = "";
        private string SignUpEmail = "";
        private string SignUpName = "";

        #region 로그인정보
        public string logInId
        {
            get { return LogInId; }
            set
            {
                LogInId = value;
                OnpropertyChanged(LogInId);
            }
        }
        public string logInPassword
        {
            get { return LogInPassword; }
            set
            {
                LogInPassword = value;
                OnpropertyChanged(LogInPassword);
            }
        }
        #endregion

        #region 회원가입정보
        public string signUpId
        {
            get { return this.SignUpId; }
            set
            {
                this.SignUpId = value;
                OnpropertyChanged(SignUpId);
            }
        }
        public string signUpPassword
        {
            get { return this.SignUpPassword; }
            set
            {
                this.SignUpPassword = value;
                OnpropertyChanged(SignUpPassword);
            }
        }
        public string signUpName
        {
            get { return this.SignUpName; }
            set
            {
                this.SignUpName = value;
                OnpropertyChanged(SignUpName);
            }
        }
        public string signUpEmail
        {
            get { return this.SignUpEmail; }
            set
            {
                this.SignUpEmail = value;
                OnpropertyChanged(SignUpEmail);
            }
        }
        #endregion

        #region ICommand
        public ICommand LogInButton { get; set; }
        public ICommand SignUpButton { get; set; }
        public ICommand IdCheckButton { get; set; }
        public ICommand PasswordCheckButton { get; set; }
        #endregion

        #region Main
        public Program()
        {
            LogInButton = new Command(dbLogIn, canExcute);
            SignUpButton = new Command(dbSignUp, canExcute);
            IdCheckButton = new Command(idCheck, canExcute);
            PasswordCheckButton = new Command(passwordCheck, canExcute);
        }
        #endregion

        #region 로그인버튼
        private void dbLogIn(object obj)
        {
            if (LogInId == "") //model에다 넣을 예정
            {
                MessageBox.Show("아이디를 입력하세요.");
            }
            else if(LogInPassword=="")
            {

            }
            database db = new database(LogInId,LogInPassword);
            bool flag = db.logIn();
        }
        #endregion

        #region 회원가입버튼
        private void dbSignUp(object obj)
        {
            database db = new database(LogInId, LogInPassword, SignUpName);
            bool flag = db.signUp();
        }
        #endregion

        #region ID 중복확인
        private void idCheck(object obj)
        {
            database db = new database(LogInId);
            bool flag = db.already();
        }
        #endregion

        protected void OnpropertyChanged(string str)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }

        private bool canExcute(object arg)
        {
            return true;
        }
    }
}
