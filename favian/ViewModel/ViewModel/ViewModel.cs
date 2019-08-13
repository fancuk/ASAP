using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DAO;
using DTO;
using System.Collections.ObjectModel;
namespace ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnpropertyChanged(string str)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }
        #endregion
        #region Main
        public ViewModel()
        {

        }
        #endregion
        #region Field
        private string LogInId = "";
        private string LogInPassword = "";
        private string SignUpId = "";
        private string SignUpPassword = "";
        private string SignUpEmail = "";
        private string SignUpName = "";
        #endregion
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
        bool IdFlag = false; //회원 돌연 아이디 변경 방지
        public string signUpId
        {
            get { return this.SignUpId; }
            set
            {
                this.SignUpId = value;
                OnpropertyChanged(SignUpId);
                IdFlag = false;
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
        public ICommand logInButton { get; set; } //로그인
        public ICommand signUpButton { get; set; } //회원가입
        public ICommand idCheckButton { get; set; } //ID증복확인
        public ICommand idChangeButton { get; set; } //ID바꾸기
        public ICommand deleteMembersButton { get; set; } //회원정보삭제
        #endregion
        #region 로그인
        private void logIn(object obj)
        {
            if (LogInId == ""||LogInPassword=="")//입력안하면 그냥 리턴
            {
                return;
            }
            info MemberLogIn = new info(LogInId, LogInPassword);
            database Database = new database();
            ObservableCollection<info> Members = Database.read();
            int cnt = Members.Count;
            for(int i = 0; i < cnt; i++)
            {
                if (Members[i].id == MemberLogIn.id &&
                    Members[i].password == MemberLogIn.password) 
                {
                    //로그인 정보 일치
                    //기능 추가
                    break;
                }
            }
        }
        #endregion
        #region 회원가입
        private void signUp(object obj)
        {
            if (SignUpId == "" || 
                SignUpPassword == "" || 
                SignUpName == "")
            {
                return;
            }
            if (IdFlag == false) return; //갑자기 바꿨다면
            database MemberInsert = new database();
            bool success = MemberInsert.create
                (SignUpId, SignUpPassword, SignUpName);
            if (success)
            {
                //회원가입 성공했다면, 기능 추가
            }
            else
            {
                //회원가입 실패, 기능 추가
            }
        }
        #endregion
        #region ID 중복확인
        private void idCheck(object obj)
        {
            bool Already = true; //이미 있는지 확인
            if (SignUpId == "") return;
            ObservableCollection<info> IdCheck = new ObservableCollection<info>();
            database Database = new database();
            IdCheck = Database.read();
            int cnt = IdCheck.Count();
            for(int i = 0; i < cnt; i++)
            {
                if(IdCheck[i].id == SignUpId)
                {
                    Already = false;
                }
            }
            if (Already == false)
            {
                //중복, 기능추가
            }
            else
            {
                //가능, 기능추가
            }
        }
        #endregion
        #region ID 바꾸기
        private void idChange(object obj)
        {
            // 나중에 구현, 바꿀 아이디 property만들어야 함
        }
        #endregion
        #region 회원정보 삭제
        private void deleteMember(object obj)
        {
            // 나중에 구현 -> 삭제할 아이디 property만들기
        }
        #endregion
    }
}
