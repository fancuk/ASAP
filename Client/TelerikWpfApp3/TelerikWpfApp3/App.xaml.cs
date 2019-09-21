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
        protected override void OnExit(ExitEventArgs e)
        {
            if (nowConnect == true)
            {
                CloseSocket();
            }
            MessageBox.Show("AA");
            base.OnExit(e);
        }
        
        public string myID;
        public Boolean mqState { get; set; }
        public void LoadMyFriends()
        {
            SendData("<FLD>", myID);
        }
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
       
        public static string nowChatTarget;

        public string getTarget()
        {
            return nowChatTarget;
        }

        public void setTarget(string tt)
        {
            nowChatTarget = tt;
        }

       ChatControl chatControl = new ChatControl();

        public void loadAllChat()
        {
            database sqlite = new database();
            sqlite.ReadChat();
        }

        public void AddSQLChat(string target, Chatitem chatitem)
        {
            chatControl.addChat(target, chatitem);
        }

        public void resetSQLChat(string target)
        {
            chatControl.resetChat(target);
        }

        IDictionary<string, ObservableCollection<Chatitem>> Chatdict 
            = new Dictionary<string, ObservableCollection<Chatitem>>();

        public static ObservableCollection<FriendsItem> FriendsList = new ObservableCollection<FriendsItem>();

        public static ObservableCollection<Chatitem> NowChat = new ObservableCollection<Chatitem>();

        public static Socket AppSock;
        
        public void setchatting(string Sender, string Receiver, string Time, string Msg)
        {
            database sqlite = new database();
            sqlite.ChattingCreate(Sender, Receiver, Time, Msg);
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
                NowChat = sqlite.ChattingRead(id) ;
                Chatdict.Add(id, NowChat);
            }
        }

        public ObservableCollection<Chatitem> getChat(string target)
        {
            NowChat = chatControl.loadChat(target);
            return NowChat;
        }

        public ObservableCollection<FriendsItem> getFriends()
        {
            return FriendsList;
        }
        public void AddChat(bool type, string text)
        {
            if (type)
            {
                //send
                NowChat.Add(new Chatitem(text, "보냄", DateTime.Now.ToString(), type));
            }
            else
            {
                NowChat.Add(new Chatitem(text, "받음", DateTime.Now.ToString(), type));
            }
        }
        public void AddFriend(string user)
        {
                FriendsList.Add(new FriendsItem(user));
        }
        MainSock Msock = new MainSock();
        public  bool nowConnect = false;
        public string nowConnectStatus = "false";
        
        public void setfriends(string friendId)
        {
            AddFriend(friendId);
        }
        public void CloseSocket()
        {
            SendData("<FIN>", "close");
            Msock.closeSock();
            nowConnect = false;
        }
        public void StartSocket()
        {
            if (nowConnect == true) return;
            Msock.UserName = "Winterlood";
            Msock.makeSock();
            if (Msock.StartConnect())
            {
                MessageBox.Show("Success to Socket Connection!");
                nowConnect = true;
            }
            else
            {
                MessageBox.Show("Socket Connection Failed!");
                nowConnect = false;
            }
        }

        public void SendData(string type, string text)
        {
            if (nowConnect == false) StartSocket();
            Msock.OnSendData(type,text);
        }
        public void SendData(string text)
        {
            if (nowConnect == false) StartSocket();
            Msock.OnSendData("<MSG>", text);
        }

        public void ShowLoginView()
        {
            Window s = TelerikWpfApp3.StartWindow.Instance;
            Window n = TelerikWpfApp3.viewtest.Instance;
            n.Show();
            s.Hide();
        }
        public void StartMainWindow()
        {
            Window s = Application.Current.MainWindow;
            Window m = TelerikWpfApp3.StartWindow.Instance;
            m.Show();
            s.Hide();
        }
    }
}
