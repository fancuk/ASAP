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
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        #region 친구 목록
        private ObservableCollection<string> Friends = 
            new ObservableCollection<string>();
        public ObservableCollection<string> friends
        {
            get { return Friends; }
            set
            {
                Friends = value;
                OnpropertyChanged("Friends");
            }
        }
        
        #endregion
        #region Client Socket
        static Socket Client;
        static byte[] Msg = new byte[100];
        #endregion
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
        private string FriendPlusID = "";
        #endregion
        #region 로그인정보
        public string logInId
        {
            get { return LogInId; }
            set
            {
                LogInId = value;
                OnpropertyChanged("LogInId");
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
        #region 친구 요청 정보
        public string friendPlusID
        {
            get { return FriendPlusID; }
            set
            {
                FriendPlusID = value;
                OnpropertyChanged("FriendPlusID");
            }
        }
        #endregion
        #region ICommand
        public ICommand logInButton { get; set; } //로그인
        public ICommand signUpButton { get; set; } //회원가입
        public ICommand idCheckButton { get; set; } //ID증복확인
        public ICommand idChangeButton { get; set; } //ID바꾸기
        public ICommand deleteMembersButton { get; set; } //회원정보삭제
        public ICommand friendPlusButton { get; set; } //회원정보삭제

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
            ObservableCollection<info> Members = 
                Database.membersRead();
            int cnt = Members.Count;
            for(int i = 0; i < cnt; i++)
            {
                if (Members[i].id == MemberLogIn.id &&
                    Members[i].password == MemberLogIn.password) 
                {
                    //로그인 정보 일치
                    //기능 추가
                    //친구목록 로드
                    friendLoad();
                    break;
                }
            }
        }
        #endregion
        #region 친구 목록 불러오기
        private void friendLoad()
        {
            database Database = new database();
            friends = Database.friendsRead(LogInId);  
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
            bool success = MemberInsert.memberCreate
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
            IdCheck = Database.membersRead();
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
        #region Client 만들기
        private void MakeClient()
        {
            Client = new Socket
                (AddressFamily.InterNetwork, 
                SocketType.Stream, ProtocolType.IP);
            Client.Connect(new IPEndPoint(IPAddress.Loopback, 11000));
            Client.BeginReceive(Msg, 0, Msg.Length,
                SocketFlags.None, new AsyncCallback(receiveMsg), Client);
        }
        #endregion
        #region receiveMsg
        private void receiveMsg(IAsyncResult a)
        {
            Socket ReceiveClient = (Socket)a.AsyncState;
            int strlen = ReceiveClient.EndReceive(a);
            string str = Encoding.Default.GetString(Msg);
            string SubString = str.Substring(0, 5);
            string FriendName = null;
            #region 친구 요청 관련
            if (SubString == "<FRR>") //친구 요청
            {
                for (int i = 5; i < strlen; i++) //친구 닉네임 추가
                {
                    if (str[i] == '님') break;
                    FriendName += str[i];
                }
                if (MessageBoxResult.Yes == //친구 요청 받으면
                    MessageBox.Show(str, "친구 요청", MessageBoxButton.YesNo))
                {
                    byte[] FriendRequest = Encoding.Default.GetBytes
                        ("<FRY>"+LogInId + "님이 친구 요청을 받았습니다.");
                    Client.BeginSend(FriendRequest, 0, FriendRequest.Length
                        , SocketFlags.None, new AsyncCallback(sendMsg), Client);
                    friends.Add(FriendName);
                    database Database = new database();
                    bool flag = Database.friendCreate(LogInId, FriendName);
                }
                else
                {
                    byte[] FriendRequest = Encoding.Default.GetBytes
                        ("<FRN>" + LogInId + "님이 친구 요청을 거절하였습니다.");
                    Client.BeginSend(FriendRequest, 0, FriendRequest.Length
                        , SocketFlags.None, new AsyncCallback(sendMsg), Client);
                }
            }
            else if (SubString == "<FRY>") //친구 요청 수락
            {
                for (int i = 5; i < strlen; i++) //친구 닉네임 추가
                {
                    if (str[i] == '님') break;
                    FriendName += str[i];
                }
                friends.Add(FriendName);
                database Database = new database();
                bool flag = Database.friendCreate(LogInId, FriendName);
            }
            else if(SubString == "<FRN>")
            {
                // 거절하면 아무것도 없다.
            }
            #endregion
            else if (SubString == "<EOF>") //메세지
            {

            }
            else if (SubString == "<LOG>") //
            {

            }
            Client.BeginReceive(Msg, 0, Msg.Length, SocketFlags.None,
                new AsyncCallback(receiveMsg), Client);
        }
        #endregion
        #region sendMsg
        private void sendMsg(IAsyncResult a)
        {
            Socket SendClient = (Socket)a.AsyncState;
            int len = SendClient.EndSend(a);
        }
        #endregion
        #region 친구요청
        private void friendPlus()
        {
            if (friendPlusID == logInId) //자기 자신 친구추가
            {
                MessageBox.Show("장난치지마세용~");
                return;
            }
            bool isit = false; // 아이디 있는지 확인
            database Database = new database();
            ObservableCollection<info> Members =
                Database.membersRead();
            int cnt = Members.Count;
            for(int i = 0; i < cnt; i++)
            {
                if (friendPlusID == Members[i].id)
                {
                    isit = true;
                    break;
                }
            }
            if (isit) // 해당 아이디 존재
            {
                byte[] FriendPlus = Encoding.Default.GetBytes
                    (LogInId + "님이 친구 요청이 왔습니다.");
                Client.BeginSend(FriendPlus, 0, FriendPlus.Length
                    , SocketFlags.None, new AsyncCallback(sendMsg), Client);
            }
            else
            {
                MessageBox.Show("해당 ID는 존재하지 않습니다.");
            }
        }
        #endregion
    }
}
