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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TelerikWpfApp3.M;
using System.Windows.Threading;
using System.Threading;

namespace TelerikWpfApp3
{
    class MainSock
    {
        public Socket mSock = null;
        public bool nowListen = false;
        public List<Socket> connectedClients = new List<Socket>();
        private string userName;
        public Boolean mqState { get; set; }
       
        delegate void STMW();
        public string UserName
        {
            get { return this.userName; }
            set
            {
                this.userName = value;
            }
        }
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

        public MainSock()
        {

        }

        public Socket makeSock()
        {
            mSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            return mSock;
        }
        
        public void closeSock()
         {
            if (((App)Application.Current).nowConnect == true)
            {
                mSock.Dispose();
                mSock.Close();
                mSock = null;
                ((App)Application.Current).nowConnect = false;
            }
        }

        public Socket bindSock(Socket sock)
        {
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, 11000);
            sock.Bind(serverEP);
            return sock;
        }

        public bool StartConnect()
        {
           //string address = "127.0.0.1";
            string address = "203.229.204.23"; // "127.0.0.1" 도 가능
            int port = 11000;
            return BeginConnection(address, port);
        }

        public bool BeginConnection(string address, int port)
        {
            try
            {
                mSock.Connect(address, port);
                //SYL.Clear();
                ////RecieveList.Add(new ListItem("[SERVER]", "안녕하세요 CLIENT 님"));
                //SYL.Add(new SYLitem("SERVER 연결 완료!.."));
                //myInfo.ipaddr = IPAddress.Loopback.ToString();
                //myInfo.port = "11000";
                //myInfo.server = "Server Is Running Now!!";
                //nowConnect = true;
                AfterConnection();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server Connect Fail!");
                return false;
            }

        }
        public string Get_MyIP()
        {
            IPHostEntry host = Dns.GetHostByName(Dns.GetHostName());
            string myip = host.AddressList[0].ToString();
            return myip;
        }

        public void AfterConnection()
        {
            AsyncObject ao = new AsyncObject(4096);
            ao.WorkingSocket = mSock;
            string ipad = Get_MyIP();
            byte[] bDts = Encoding.UTF8.GetBytes("<SOF>" + '/' + ipad);
            ao.WorkingSocket.Send(bDts);
            ao.WorkingSocket.BeginReceive(ao.Buffer, 0, ao.BufferSize, 0, DataReceived, ao);
        }

        public void RunServer(Socket sock)
        {
            if (this.nowListen == false)
            {
                sock.Listen(10);
                sock.BeginAccept(AcceptCallback, null);
                this.nowListen = true;
                MessageBox.Show("Now Server is Run");
                Console.WriteLine("Now Server is Run");
            }
            else
            {
                MessageBox.Show("Already SErver is Run");
            }
        }

        #region AcceptCallBack
        void AcceptCallback(IAsyncResult ar)
        {
            if (!nowListen)
            {
                mSock.BeginAccept(AcceptCallback, null);
                return;
            }
            // 클라이언트의 연결 요청을 수락한다.
            Socket client = mSock.EndAccept(ar);
            if (!client.Connected)
            {
                return;
            }
            AsyncObject obj = new AsyncObject(4096);
            obj.WorkingSocket = client;
            // 연결된 클라이언트 리스트에 추가해준다.
            connectedClients.Add(client);

            client.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
            mSock.BeginAccept(AcceptCallback, null);

            nowListen = true;
        }
        #endregion

        #region DataReceived
        void DataReceived(IAsyncResult ar)
        {
            AsyncObject obj = (AsyncObject)ar.AsyncState;

            try
            {
                //int received = obj.WorkingSocket.EndReceive(ar);
                //if (received <= 0)
                //{
                //    obj.WorkingSocket.Close();
                //    return;
                //}

                // UTF8 인코더를 사용하여 바이트 배열을 문자열로 변환한다.
                if (!mSock.Connected)
                {
                    MessageBox.Show("Server is now ShutDown!");
                    return;
                }
                string text = Encoding.UTF8.GetString(obj.Buffer);

                // 0x01 기준으로 짜른다.
                // tokens[0] - 보낸 사람 IP
                // tokens[1] - 보낸 메세지
                string[] tokens = text.Split('/');
                string tag = tokens[0];
                if (tokens.Length == 1) return;
                if (tag.Equals("<LOG>")) // 로그인
                {
                    string flag = tokens[1];
                    if (flag.Equals("true"))
                    {
                        Properties.Settings.Default.loginOK = true;
                        DispatchService.Invoke(() =>
                        {
                            ((App)Application.Current).StartMainWindow();
                        });
                        nowListen = true;
                        string myId = ((App)Application.Current).getmyID();

                        Thread.Sleep(10);

                        byte[] bDts = null;
                        string str = "<FLD>" + '/' +myId  + '/';

                        bDts = Encoding.UTF8.GetBytes(str);
                        mSock.Send(bDts);
                    }
                    else
                    {
                        MessageBox.Show("Login Failed.....TT");
                        Properties.Settings.Default.loginOK = false;
                        ((App)Application.Current).CloseSocket();
                    }
                }
                else if (tag.Equals("<REG>")) // 회원가입
                {
                    string flag = tokens[1];
                    if (flag.Equals("true"))
                    {
                        Window s = new SuccessMsgBox("회원가입 성공");
                        DispatchService.Invoke(() =>
                        {
                            ((App)Application.Current).StartMainWindow();

                        });
                        s.Show();
                    }
                    else
                    {
                        MessageBox.Show("Register Failed.....TT");
                        Window x = new FalseMsgBox("Fail!");
                        DispatchService.Invoke(() => //너무 많은 UI 어쩌구저쩌구 SPA? STA 나와서 invoke 처리 2019-09-19 다민
                        {
                            x.Show();
                        });
                    }
                    ((App)Application.Current).CloseSocket();
                }
                else if (tag.Equals("<ICF>")) // ID 체크
                {
                    string flag = tokens[1];
                    if (flag.Equals("true"))
                    {
                        MessageBox.Show("ID Check Sucess! in view");
                        DispatchService.Invoke(() =>
                        {
                            ((App)Application.Current).StartMainWindow();
                        });
                         ((App)Application.Current).setidchk(true);
                    }
                    else
                    {
                        MessageBox.Show("ID Check Failed.....TT");
                    }
                    ((App)Application.Current).CloseSocket();
                }
                else if (tag.Equals("<FRR>"))
                {
                    if (tokens[1] == "true")
                    {
                        string target = tokens[2];
                        DispatchService.Invoke(() =>
                        {
                            ((App)Application.Current).setfriends(target);
                        });
                        MessageBox.Show("친구 추가 되었습니다!");
                    }
                    else
                    {
                        MessageBox.Show("존재하지 않는 ID입니다.");
                    }
                }
                /*else if (tag.Equals("<FRR>")) // 친구추가
                {
                    if (MessageBoxResult.Yes == //친구 요청 받으면
                    MessageBox.Show(tokens[1] + tokens[3], "친구 요청", MessageBoxButton.YesNo))
                    {
                        OnSendData("<FRA>", tokens[1] + "/" + tokens[2]+ "/Yes/");
                    }
                    else
                    {
                        OnSendData("<FRA>", tokens[1] + "/" + tokens[2] + "/No/");
                    }
                    DispatchService.Invoke(() =>
                    {
                        ((App)Application.Current).StartMainWindow();
                    });
                }
                else if (tag.Equals("<FRA>")) // 친구 feedback
                {
                    MessageBox.Show(tokens[2] + tokens[3]);
                    DispatchService.Invoke(() =>
                    {
                        ((App)Application.Current).StartMainWindow();
                    });
                }
                */
                else if (tag.Equals("<MSG>")) // 메세지
                {
                    Chatitem tmp = new Chatitem();
                    tmp.User = tokens[1];
                    tmp.Time = tokens[3];
                    tmp.Text = tokens[4];
                    ((App)Application.Current).setchatting(tokens[1], tokens[2], tokens[3], tokens[4]);
                    DispatchService.Invoke(() =>
                    {
                        ((App)Application.Current).AddSQLChat(tmp.User, tmp);
                    });
                    if (!((App)Application.Current).mqState)
                    {
                        ((App)Application.Current).resetSQLChat(tmp.User);
                    }

                }
                else if (tag.Equals("<FIN>"))
                {
                    if (((App)Application.Current).nowConnect == true)
                    {
                        closeSock();
                    }
                }
                else if (tag.Equals("<FLD>"))
                {
                    ((App)Application.Current).setfriends(tokens[1]);
                }
                // 텍스트박스에 추가해준다.
                // 비동기식으로 작업하기 때문에 폼의 UI 스레드에서 작업을 해줘야 한다.
                // 따라서 대리자를 통해 처리한다.

                //^^^^^^^^
                // 클라이언트에선 데이터를 전달해줄 필요가 없으므로 바로 수신 대기한다.
                // 데이터를 받은 후엔 다시 버퍼를 비워주고 같은 방법으로 수신을 대기한다.
                if (((App)Application.Current).nowConnect == true) //예외 처리 obj beginreceive
                {
                    obj.ClearBuffer();

                    // 수신 대기
                    obj.WorkingSocket.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
                }
            }
            catch (Exception e)
            {
                if (((App)Application.Current).nowConnect == true)
                {
                    closeSock();
                }
                MessageBox.Show("서버와의 연결 오류!");
                return;
            }
        }
        #endregion

        #region OnSendData
        public void OnSendData(string type, string Texts)
        {
            // 보낼 텍스트
            string tts = Texts.Trim();

            byte[] bDts = null;
            string str = type + '/' + tts + '/';

            bDts = Encoding.UTF8.GetBytes(str);
            mSock.Send(bDts);
        }
        #endregion

    }
}