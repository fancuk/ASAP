using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChatWindow : Window
    {
        public class ListItem
        {
            public string text { get; set; }
            public string user { get; set; }
            public string time { get; set; }
            public ListItem(string user, string text,string time)
            {
                this.text = text;
                this.user = user;
                this.time = time;
            }
        }
        public class SYLitem
        {
            public string log { get; set; }
            public SYLitem(string value)
            {
                this.log = value;
            }
        }
        public class info : INotifyPropertyChanged
        {
            private string ID;
            private string PORT;
            private string IPADDR;
            private string SERVER;
            public string id
            {
                get { return ID; }
                set { this.ID = value; OnPropertyChanged("id"); }
            }
            public string port
            {
                get { return PORT; }
                set { this.PORT = value; OnPropertyChanged("port"); }
            }
            public string ipaddr
            {
                get { return IPADDR; }
                set { this.IPADDR = value; OnPropertyChanged("ipaddr"); }
            }
            public string server
            {
                get { return SERVER; }
                set { this.SERVER = value; OnPropertyChanged("server"); }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
       public void clearList(bool syl, bool send, bool recieve)
        {
            if(syl)SYL.Clear();
            if (send) ChatList.Clear();
            if (recieve) ChatList.Clear();
            myInfo.server = "Server is Shutdown";
        }
        delegate void AddListItem(string id, string text, Boolean flag);
        delegate void AddSYL(string text);
        delegate void ClearAllItem(bool syl, bool send, bool recieve);
        public void addSyl(string text)
        {
            SYL.Add(new SYLitem(text));
        }
        public void AddLists(string id, string text, Boolean flag)
        {
            if (flag)
            {
                ChatList.Add(new ListItem(null, text, DateTime.Now.ToString("HH:mm:ss")));
                ChatBox.UpdateLayout();
              //  SendBox.ScrollIntoView(SendBox.Items[SendBox.Items.Count - 1]);

            }
            else
            {
                ChatList.Add(new ListItem(id, text, DateTime.Now.ToString("HH:mm:ss")));
                ChatBox.UpdateLayout();
       //         RecieveBox.ScrollIntoView(RecieveBox.Items[RecieveBox.Items.Count - 1]);

            }
        }
        public ObservableCollection<SYLitem> SYL = new ObservableCollection<SYLitem>();

        public ObservableCollection<ListItem> ChatList = new ObservableCollection<ListItem>();

        Socket mainSock;
        public class AsyncObject
        {
            public byte[] Buffer;
            public Socket WorkingSocket;
            public readonly int BufferSize;
            public AsyncObject(int bufferSize)
            {
                BufferSize = bufferSize;
                Buffer = new byte[BufferSize];
            }

            public void ClearBuffer()
            {
                Array.Clear(Buffer, 0, BufferSize);
            }
        }


        void DataReceived(IAsyncResult ar)
        {
            // BeginReceive에서 추가적으로 넘어온 데이터를 AsyncObject 형식으로 변환한다.
            AsyncObject obj = (AsyncObject)ar.AsyncState;
            if (!mainSock.Connected)
            {
                myInfo.server = "Server is Shutdown";
                return;
            }
            // 데이터 수신을 끝낸다.
            try
            {
                int received = obj.WorkingSocket.EndReceive(ar);
                if (received <= 0)
                {
                    obj.WorkingSocket.Close();
                    return;
                }

                // UTF8 인코더를 사용하여 바이트 배열을 문자열로 변환한다.
                string text = Encoding.UTF8.GetString(obj.Buffer);

                // 0x01 기준으로 짜른다.
                // tokens[0] - 보낸 사람 IP
                // tokens[1] - 보낸 메세지
                string[] tokens = text.Split('\x01');
                string ip = tokens[0];
                string msg = tokens[1];

                // 텍스트박스에 추가해준다.
                // 비동기식으로 작업하기 때문에 폼의 UI 스레드에서 작업을 해줘야 한다.
                // 따라서 대리자를 통해 처리한다.

                Console.WriteLine("Server Send : {0}", msg);
                //^^^^^^^^
                AddListItem a = new AddListItem(AddLists);
                this.ChatBox.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new AddListItem(a), ip, msg, false);

                // 클라이언트에선 데이터를 전달해줄 필요가 없으므로 바로 수신 대기한다.
                // 데이터를 받은 후엔 다시 버퍼를 비워주고 같은 방법으로 수신을 대기한다.
                obj.ClearBuffer();

                // 수신 대기
                obj.WorkingSocket.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
            }
            catch(Exception e)
            {
                nowConnect = false;
                ClearAllItem cai = new ClearAllItem(clearList);
                AddSYL asyl = new AddSYL(addSyl);
                this.ChatBox.Dispatcher.BeginInvoke
                    (System.Windows.Threading.DispatcherPriority.Normal, 
                    new ClearAllItem(cai), true, true, true);
                this.ChatBox.Dispatcher.BeginInvoke
                 (System.Windows.Threading.DispatcherPriority.Normal,
                 new AddSYL(asyl), "현재 서버가 연결을 종료했습니다!");
                nowConnect = false;
                obj.WorkingSocket.Close();
                return;
            }
            // 받은 데이터가 없으면(연결끊어짐) 끝낸다.
        
        }
        info myInfo = new info();
        public ChatWindow()
        {
            mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            InitializeComponent();
            DataContext = myInfo;
            myInfo.server = "Not Running Now";
            myInfo.id = Application.Current.Properties["id"].ToString();
            this.ChatBox.ItemsSource = ChatList;
            this.SytemLogList.ItemsSource = SYL;
        }
        public void userInputText_KeyDown(object sender, RoutedEventArgs e)
        {

        }
        public void click_sendMessage(object sender, RoutedEventArgs e)
        {
            AddLists("[보냄]", inputText.Text.ToString(), true);

            OnSendData();
        }
        void OnSendData()
        {
            // 서버가 대기중인지 확인한다.
            if (!mainSock.IsBound)
            {
                MessageBox.Show("서버가 실행되고 있지 않습니다!");
                return;
            }

            // 보낼 텍스트
            string tts = inputText.Text.Trim();
            if (string.IsNullOrEmpty(tts))
            {
                MessageBox.Show("텍스트 입력 바람!");
                inputText.Focus();
                return;
            }

            // 서버 ip 주소와 메세지를 담도록 만든다.
            IPEndPoint ip = (IPEndPoint)mainSock.LocalEndPoint;
            string addr = ip.Address.ToString();

            // 문자열을 utf8 형식의 바이트로 변환한다.
            byte[] bDts = Encoding.UTF8.GetBytes(userName.Text + '\x01' + tts);

            // 서버에 전송한다.
            mainSock.Send(bDts);
            Console.WriteLine("전송 : {0}", inputText.Text);
            // 전송 완료 후 텍스트박스에 추가하고, 원래의 내용은 지운다.
            inputText.Clear();
        }
        public void MakeSocket()
        {
            string address = "localhost"; // "127.0.0.1" 도 가능
            int port = 11000;
            BeginConnection(address, port);
        }

        public void AfterConnection()
        {
            AsyncObject ao = new AsyncObject(4096);
            ao.WorkingSocket = mainSock;
            byte[] bDts = Encoding.UTF8.GetBytes(userName.Text + '\x01'+"<SOF>");
            mainSock.Send(bDts);
            mainSock.BeginReceive(ao.Buffer, 0, ao.BufferSize, 0, DataReceived, ao);
        }
        public void BeginConnection(string address, int port)
        {
            try
            {
                mainSock.Connect(address, port);
                SYL.Clear();
                //RecieveList.Add(new ListItem("[SERVER]", "안녕하세요 CLIENT 님"));
                SYL.Add(new SYLitem("SERVER 연결 완료!.."));
                myInfo.ipaddr = IPAddress.Loopback.ToString();
                myInfo.port = "11000";
                myInfo.server = "Server Is Running Now!!";
                nowConnect = true;
                AfterConnection();
            }
            catch (Exception e)
            {
                ChatList.Clear();
                SYL.Clear();
                SYL.Add(new SYLitem("서버 연결에 실패했습니다....."));
            }

        }
        bool nowConnect = false;
        public void startServer(object sender, RoutedEventArgs e)
        {
            MakeSocket();
        }
        public void stopServer(object sender, RoutedEventArgs e)
        {
            if (!nowConnect)
            {
                SYL.Add(new SYLitem("현재 연결중이지 않습니다..."));
                return;
            }
          mainSock.Shutdown(SocketShutdown.Both);
            mainSock.Close();
        }
    }
}
