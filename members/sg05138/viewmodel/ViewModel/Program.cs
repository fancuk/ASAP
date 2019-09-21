using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MembersDAO;
using MembersDTO;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Data.SQLite;

namespace ViewModel
{
    class Program : INotifyPropertyChanged
    {
        static void Main(string[] args)
        {

        }

        #region Field
        private string LogInId = "";
        private string LogInPassword = "";
        private string SignUpId = "";
        private string SignUpPassword = "";
        private string SignUpEmail = "";
        private string SignUpName = "";
        private string MessageText = "";
        static Socket SendSock;
        static Socket GetSock;
        static byte[] Msg = new byte[100];
        bool LoginOk = false;
        bool SqlCheck = false;
        #endregion

        #region Msg 받기 종료
        public void getStr(IAsyncResult a)
        {
            GetSock = (Socket)a.AsyncState;
            int strLength = GetSock.EndReceive(a);
            string str = Encoding.Default.GetString(Msg);
        }
        #endregion

        #region Msg 전송 종료
        public void sendStr(IAsyncResult a)
        {
            Socket tmpSock = (Socket)a.AsyncState;
            int strlen = tmpSock.EndSend(a);
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnpropertyChanged(string str)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }
        #endregion

        #region Main
        public Program()
        {
            logInButton = new Command(logIn, canExcute);
            signUpButton = new Command(signUp, canExcute);
            idCheckButton = new Command(idCheck, canExcute);
            pwdChangeButton = new Command(pwdChange, canExcute);
            deleteMembersButton = new Command(deleteMembers, canExcute);
            messageSendButton = new Command(sendMessage, canExcute);
        }
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

        #region msgtextbox내용
        public string messageText
        {
            get { return MessageText; }
            set
            {
                MessageText = value;
                OnpropertyChanged(MessageText);
            }
        }
        #endregion

        #region 메세지보내기
        private void sendMessage(object obj)
        {
            Socket tmpSock = SendSock.Accept();
            byte[] Msg = Encoding.Default.GetBytes("<MSG>"+ LogInId + MessageText);
            tmpSock.BeginSend(Msg, 0, 11, SocketFlags.None, new AsyncCallback(sendStr), tmpSock);
        }
        #endregion

        #region ICommand
        public ICommand logInButton { get; set; } //로그인
        public ICommand signUpButton { get; set; } //회원가입
        public ICommand idCheckButton { get; set; } //ID증복확인
        public ICommand pwdChangeButton { get; set; } //ID바꾸기
        public ICommand deleteMembersButton { get; set; } //회원정보삭제
        public ICommand messageSendButton { get; set; } //메세지전송
        #endregion

        #region 로그인 & client 소캣 생성
        private void logIn(object obj)
        {
            if (LogInId == "" || LogInPassword == "")//입력안하면 그냥 리턴
            {
                return;
            }
            info MemberLogIn = new info(LogInId, LogInPassword);
            database Database = new database();
            ObservableCollection<info> Members = Database.read();
            int cnt = Members.Count;
            for (int i = 0; i < cnt; i++)
            {
                if (Members[i].id == MemberLogIn.id && Members[i].password == MemberLogIn.password)
                {
                    LoginOk = true;
                    //로그인 정보 일치
                    //기능 추가
                    break;
                }
            }
            if (LoginOk == true)
            {
                GetSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                GetSock.Connect(new IPEndPoint(IPAddress.Loopback, 10801));//서버주소로 바꾸기
                //서버와 연결됨
                string DBfile = @"C:\Users\Chatting.db";
                SQLiteConnection SqlConn = new SQLiteConnection(@"Data Source = C:\Users\Chatting.db;VErsion=3;");
                if (!System.IO.File.Exists(DBfile))
                {
                    SQLiteConnection.CreateFile(@"C:\Users\Chatting.db");
                    SqlCheck = true;
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
            bool success = MemberInsert.create(SignUpId, SignUpPassword, SignUpName);
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
            for (int i = 0; i < cnt; i++)
            {
                if (IdCheck[i].id == SignUpId)
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

        #region PWD 바꾸기
        private void pwdChange(object obj)
        {
            // 나중에 구현, 바꿀 아이디 property만들어야 함
        }
        #endregion

        #region 회원정보 삭제
        private void deleteMembers(object obj)
        {
            // 나중에 구현 -> 삭제할 아이디 property만들기
        }
        #endregion
      
        private bool canExcute(object arg)
        {
            return true;
        }
    }
}
