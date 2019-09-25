using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Windows;
using TelerikWpfApp3.Controller;
using TelerikWpfApp3.M;
using TelerikWpfApp3.VM;
using TelerikWpfApp3.VM.DBControl;

namespace TelerikWpfApp3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            mqState = false;
            Startup += App_Startup;
            InitializeComponent();
        }
        private void App_Startup(object sender, StartupEventArgs e)
        {
            TelerikWpfApp3.viewtest.Instance.Show();
        }
        public Boolean mqState { get; set; }

        #region 프로퍼티

        public string myID;
       
        public string getmyID()
        {
            return myID;
        }
        public void setmyID(string myid)
        {
            myID = myid;
        }
        public bool idchk;
        public bool getidchk()
        {
            return idchk;
        }
        public void setidchk(bool isit)
        {
            idchk = isit;
        }
        public bool emailChk;
        public bool getEmailChk()
        {
            return emailChk;
        }
        public void setEmailChk(bool email)
        {
            emailChk = email;
        }
        public static string nowChatTarget;

        public string getTarget()
        {
            return nowChatTarget;
        }

        public void setTarget(string tt)
        {
            nowChatTarget = tt;
        }
        #endregion
        #region chattingControl
        public ChatControl chatControl = new ChatControl();
        ChattingController chattingController = new ChattingController();
        public IDictionary<string, ObservableCollection<Chatitem>> Chatdict
            = new Dictionary<string, ObservableCollection<Chatitem>>();

        public ObservableCollection<Chatitem> NowChat = new ObservableCollection<Chatitem>();

        public void loadAllChat()
        {
            chattingController.loadAllChat();
        }

        public void AddSQLChat(string target, Chatitem chatitem)
        {
            chattingController.AddSQLChat(target, chatitem);
        }

        public void resetSQLChat(string target)
        {
            chattingController.resetSQLChat(target);
        }      
        public void setchatting(string Sender, string Receiver, string Time, string Msg)
        {
            chattingController.setchatting(Sender, Receiver, Time, Msg);
        }


        public void setChat(string id)
        {
            chattingController.setChat(id);
        }

        public ObservableCollection<Chatitem> getChat(string target)
        {
            return chattingController.getChat(target);
        }

        
        public void AddChat(bool type, string text)
        {
            chattingController.AddChat(type, text);
        }
        #endregion
        #region FriendsController
        public ObservableCollection<FriendsItem> FriendsList = new ObservableCollection<FriendsItem>();
        FriendsController friendsController = new FriendsController();
        public ObservableCollection<FriendsItem> getFriends()
        {
            return friendsController.getFriends();
        }
        public void LoadMyFriends()
        {
            friendsController.LoadMyFriends(myID);
        }
        public void AddFriend(string user)
        {
            friendsController.AddFriend(user);
        }
                
        public void setfriends(string friendId)
        {
            friendsController.AddFriend(friendId);
        }
        #endregion
        #region SocketController
        public MainSock Msock = new MainSock();
        public bool nowConnect = false;
        public string nowConnectStatus = "false";
        SocketController socketController = new SocketController();
        public static Socket AppSock;
        public void CloseSocket()
        {
            socketController.CloseSocket();
        }
        public void StartSocket()
        {
            socketController.StartSocket();
        }
        
        public void SendData(string type, string text)
        {
            socketController.SendData(type, text);
        }
        public void SendData(string text)
        {
            socketController.SendData(text);
        }
        #endregion
        #region WindowController
        WindowController windowController = new WindowController();
        public void ShowLoginView()
        {
            windowController.ShowLoginView();
        }
        public void StartMainWindow()
        {
            windowController.StartMainWindow();
        }
        #endregion
    }
}
