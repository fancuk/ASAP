using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Windows;
using TelerikWpfApp3.M;
using TelerikWpfApp3.Networking;
using TelerikWpfApp3.Utility;
using TelerikWpfApp3.View.Alert;
using TelerikWpfApp3.VM;
using TelerikWpfApp3.VM.DBControl;

namespace TelerikWpfApp3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        DabbingPreventor dabbingPreventor = DabbingPreventor.Instance;
        public bool nowConnect = false;
        public string nowConnectStatus = "false";
        SocketConnector socketConnector;
        SocketSender socketSender;
        SocketCloser socketCloser;
        ChatControl chatControl = new ChatControl();
        WindowManager windowManager = new WindowManager();

        IDictionary<string, ObservableCollection<Chatitem>> Chatdict    = new Dictionary<string, ObservableCollection<Chatitem>>();

        //다민 public static ObservableCollection<FriendsItem> FriendsList = new ObservableCollection<FriendsItem>();


        public static ObservableCollection<Chatitem> NowChat = new ObservableCollection<Chatitem>();
        public string myID { get; set; }
        public Boolean mqState { get; set; }
        public Socket ProgramSock { get; set; }
        public bool idchk { get; set; }
        public bool emailChk { get; set; }
        public string nowChatTarget { get; set; }
        //public Boolean loadAllChk = false; 다민
        public App()
        {
            mqState = false;
            Startup += App_Startup;
            InitializeComponent();
        }

        #region initSocketInstance
        public void InitSocketInstance()
        {
             socketConnector = new SocketConnector();
             socketSender = new SocketSender();
             socketCloser = new SocketCloser();
        }
        #endregion

        #region StartSocket
        public bool StartSocket()
        {
            makeSocket();
            return socketConnector.SocketConnect();
        }
        #endregion

        #region makeSocket
        public void makeSocket()
        {
            SocketMaker sm = new SocketMaker();
            ProgramSock = sm.makeSock(ProgramSock);
            InitSocketInstance();
        }
        #endregion

        #region send
        public void SendData(string type, string text)
        {
          
            if (nowConnect == false) StartSocket();
            socketSender.OnSendData(type, text);
        }
        #endregion

        #region CloseSocket
        public void CloseSocket()
        {
            socketCloser.closeSock();
        }
        #endregion

        private void App_Startup(object sender, StartupEventArgs e)
        {
            TelerikWpfApp3.viewtest.Instance.Show();
        }
        public void AddSQLChat(string target, Chatitem chatitem)
        {
            //setchatting(chatitem.User, myID, chatitem.Time, chatitem.Text);
            chatControl.addChat(target, chatitem);
        }

        public void resetSQLChat(string target)
        {
            chatControl.resetChat(target);
        }
        public void setchatting(string Sender, string Receiver, string Time, string Msg,string type)
        {
            database sqlite = new database();
            sqlite.ChattingCreate(Sender, Receiver, Time, Msg,type);
        }

        public void setChat(string id)
        {
            database sqlite = new database();
            if (Chatdict.ContainsKey(id))
            {
            }
            else
            {
                //sqllite load
                NowChat = sqlite.ChattingRead(myID,id) ;
                Chatdict.Add(id, NowChat);
            }
        }

        public void loadAllChat()
        {
            database sqlite = new database();
            sqlite.ReadChat();
            int count = FriendsUserControlViewModel.Instance.FriendsList.Count;
            for (int i = 0; i < count; i++)  // 다민
            {
                FriendsUserControlViewModel.Instance.FriendsList[i].LastMesseage =
                    chatControl.getLastChatById(FriendsUserControlViewModel.Instance.FriendsList[i].User);
            }
        }
        public void loadAllChatList()
        {
            database sqlite = new database();
            sqlite.ReadChat();
            foreach(KeyValuePair<string,ObservableCollection<Chatitem>> items in Chatdict)
            {
                
            }
        }
        public ObservableCollection<Chatitem> getChat(string target)
        {
            NowChat = chatControl.loadChat(target);
            return NowChat;
        }

        /*public ObservableCollection<FriendsItem> getFriends()
        {
            return FriendsList;
        }다민*/

        /*public void AddFriend(string user, string _status)
        {
            //if (_status == "true")
            //{
                FriendsList.Add(new FriendsItem(user, null, "true"));
            //}
            //else
            //{
            //    FriendsList.Add(new FriendsItem(user, null, "false"));
            //}
        }다민*/

        /*public void ChangeStatus(string User,string _status)
        {
            for(int i = 0; i < FriendsList.Count; i++)
            {
                if (FriendsList[i].User == User)
                {
                    FriendsList[i].Status = _status;
                }
            }
        }다민*/
        //친구 추가 중복 체크
        /*public bool FriendDoubleCheck(string user)
        {
            for (int i = 0; i < FriendsList.Count; i++)
            {
                if (FriendsList[i].User == user)
                    return true;
            }
            return false;
        }다민*/


        public void ShowLoginView()
        {
            windowManager.ShowLoginView();
        }
        public void StartMainWindow()
        {
            windowManager.StartMainWindow();
        }
        public void RegisterComplete()
        {
            windowManager.RegisterComplete();
        }
    }
}
