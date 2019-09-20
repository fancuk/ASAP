using DBConn;
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

namespace TelerikWpfApp3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            ChatList.Add(new Chatitem("안녕하세요 server입니다 병신아.", "server", "19:20:22", false));
            ChatList.Add(new Chatitem("안녕하세요 Client 입니다 병신아.", "Client", "19:20:22", true));
            ClientList.Add(new ClientItem("SERVER", "SERVER입니다 반갑습니다.", true));
            ClientList.Add(new ClientItem("ME", "나 자신임", false));
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
        public class Chatitem
        {
            private string text;
            private string user;
            private string time;
            private bool chk;
            public Chatitem(string text, string user, string time, bool chk)
            {
                this.Text = text;
                this.User = user;
                this.Time = time;
                this.Chk = chk;
            }

            public string Text { get => text; set => text = value; }
            public string User { get => user; set => user = value; }
            public string Time { get => time; set => time = value; }
            public bool Chk { get => chk; set => chk = value; }
        }
        public string myID;

        public string getmyID()
        {
            return myID;
        }
        public void setmyID(string myid)
        {
            myID = myid;
        }
        public class ClientItem
        {
            private string user;
            private string status;
            private bool chk;

            public ClientItem(string user, string status, bool chk)
            {
                this.User = user;
                this.Status = status;
                this.Chk = chk;
            }

            public string User { get => user; set => user = value; }
            public string Status { get => status; set => status = value; }
            public bool Chk { get => chk; set => chk = value; }
        }


        public static ObservableCollection<ClientItem> ClientList = new ObservableCollection<ClientItem>();

        public static ObservableCollection<Chatitem> ChatList = new ObservableCollection<Chatitem>();
        public static Socket AppSock;
        
        public void setchatting(string Sender, string Receiver, string Time, string Msg)
        {
            database sqlite = new database();
            sqlite.ChattingCreate(Sender, Receiver, Time, Msg);
        }
        public ObservableCollection<Chatitem> getChat()
        {
            return ChatList;
        }
        public ObservableCollection<ClientItem> getClient()
        {
            return ClientList;
        }
        public void AddChat(bool type, string text)
        {
            if (type)
            {
                //send
                ChatList.Add(new Chatitem(text, "보냄", DateTime.Now.ToString(), type));
            }
            else
            {
                ChatList.Add(new Chatitem(text, "받음", DateTime.Now.ToString(), type));
            }
        }
        public void AddClient(bool type, string user,string status)
        {
            if (type)
            {
                //send
                ClientList.Add(new ClientItem(user, "상태메세지 들어갈곳",  type));
            }
            else
            {
                ClientList.Add(new ClientItem(user, "상태메세지 들어갈곳", type));
            }
        }
        ChatList chatList;
        MainSock Msock = new MainSock();
        public  bool nowConnect = false;
        public string nowConnectStatus = "false";
        FriendsList friends = new FriendsList();
        
        public void setfriends(string friendId)
        {
            friends.setFriends(friendId);
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
            AddChat(true, text);
        }

        public void ShowLoginView()
        {
            Window sts = Application.Current.MainWindow;
            Window n = new viewtest();
            try
            {
                n.Show();
                sts.Hide();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        public  void StartMainWindow()
        {
            Window s = Application.Current.MainWindow;
            Window m = new StartWindow();
            m.Show();
            s.Close();
        }
    }
}
