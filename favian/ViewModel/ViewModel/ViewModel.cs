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

namespace ViewModel //WPF에서 Flag 저장해주기.
                    //최초의 로그인 -> 친구목록SQLite File, 대화목록SQLite File 생성
                    //Flag = false;
                    //문제가 생김 -> 다른 컴퓨터에서 로그인은 어쩔껀지?
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
            if (signUpId == "" || signUpPassword == "" ||)
            {
                MessageBox.Show("빈칸 금지");
                return;
            }
            byte[] LogInMsg = Encoding.Default.GetBytes
                ("<LOG>/" + logInId + "/" + logInPassword);
            Client.BeginSend(LogInMsg, 0, LogInMsg.Length, SocketFlags.None,
                new AsyncCallback(sendMsg), Client);
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
            if (signUpId == "" || signUpPassword == "" ||
                signUpName == "")
            {
                MessageBox.Show("빈칸 금지");
                return;
            }
            if (IdFlag)
            {
                byte[] LogInMsg = Encoding.Default.GetBytes
                     ("<SUP>/" + signUpId + "/" + signUpPassword + "/" + signUpName); ;
                Client.BeginSend(LogInMsg, 0, LogInMsg.Length, SocketFlags.None,
                    new AsyncCallback(sendMsg), Client);
                MessageBox.Show("회원가입 성공.");
            }
            else
            {
                MessageBox.Show("중복확인 해주세요.");
            }
        }
        #endregion
        #region ID 중복확인
        private void idCheck(object obj)
        {
            byte[] LogInMsg = Encoding.Default.GetBytes
                 ("<CHK>/" + signUpId); ;
            Client.BeginSend(LogInMsg, 0, LogInMsg.Length, SocketFlags.None,
                new AsyncCallback(sendMsg), Client);
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
                MessageBox.Show(str); //<FRY> 뺴야함
                for (int i = 5; i < strlen; i++) //친구 닉네임 추가
                {
                    if (str[i] == '님') break;
                    FriendName += str[i];
                }
                friends.Add(FriendName);
                database Database = new database();
                bool flag = Database.friendCreate(LogInId, FriendName);
            }
            else if(SubString == "<FRN>") //거절
            {
                // 거절하면 아무것도 없다.
            }
            #endregion
            else if (SubString == "<FNE>") //아이디 존재x
            {
                MessageBox.Show("해당 아이디는 존재하지 않습니다.");
            }
            else if (SubString == "<LOY>") //로그인 가능
            {
                MessageBox.Show("로그인 성공");
                // 메인 화면 띄우기
                friendLoad();
            }
            else if (SubString == "<LON>")
            {
                MessageBox.Show("아이디, 비밀번호를 확인해주세요.");
            }
            else if (SubString == "CHY")
            {
                MessageBox.Show("아이디 사용 가능");
                IdFlag = true;
            }
            else if (SubString == "CHN")
            {
                MessageBox.Show("사용중입니다.");
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
            byte[] LogInMsg = Encoding.Default.GetBytes
                 ("<FRR>/" + signUpId); ;
            Client.BeginSend(LogInMsg, 0, LogInMsg.Length, SocketFlags.None,
                new AsyncCallback(sendMsg), Client);
        }
        #endregion
    }
}
