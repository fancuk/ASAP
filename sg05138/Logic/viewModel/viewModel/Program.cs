using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using memberVO;

namespace viewModel
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
        public ICommand LogInButton { get; set; } //로그인
        public ICommand SignUpButton { get; set; } //회원가입
        public ICommand IdCheckButton { get; set; } //ID증복확인
        public ICommand PasswordCheckButton { get; set; } //PWD확인
        public ICommand ChangePwdButton { get; set; } //PWD 변경
        public ICommand DeleteMembersButton { get; set; } //회원정보삭제
        #endregion

        #region Main
        public Program()
        {
            LogInButton = new Command(dbLogIn, canExcute);
            SignUpButton = new Command(dbSignUp, canExcute);
            IdCheckButton = new Command(idCheck, canExcute);
            ChangePwdButton = new Command(pwdChange, canExcute);
            DeleteMembersButton = new Command(deleteMembers, canExcute);
            //PasswordCheckButton = new Command(passwordCheck, canExcute);
        }
        #endregion

        #region 로그인버튼
        private void dbLogIn(object obj)
        {
            if (LogInId == "")
            {
                MessageBox.Show("아이디를 입력하세요.");
                return;
            }
            else if (LogInPassword == "")
            {
                MessageBox.Show("비밀번호를 입력하세요.");
                return;
            }

        }
        #endregion

        #region 회원가입버튼
        private void dbSignUp(object obj)
        {

        }
        #endregion

        #region ID 중복확인
        private void idCheck(object obj)
        {
           
        }
        #endregion

        #region PWD 변경
        private void pwdChange(object obj)
        {

        }
        #endregion

        #region 회원정보 삭제
        private void deleteMembers(object obj)
        {

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

        static void Main(string[] args)
        {

        }
    }
}
