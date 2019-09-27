using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static TelerikWpfApp3.MainSocket.SocketReceived;

namespace TelerikWpfApp3.MainSocket
{
    public class SocketConnecter
    {
        public bool nowListen = false;
        public List<Socket> connectedClients = new List<Socket>();
        public Boolean mqState { get; set; }
        SocketReceived sReceive = new SocketReceived();

        public bool StartConnect()
        {
            //string address = "127.0.0.1";
            string address = "203.229.204.23"; // "127.0.0.1" 도 가능
            int port = 11000;
            return BeginConnection(address, port);
        }

        public SocketConnecter()
        {
            
        }

        public bool BeginConnection(string address, int port)
        {
            try
            {
                SocketController.mSock.Connect(address, port);
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

        public void AfterConnection()
        {
            AsyncObject ao = new AsyncObject(4096);
            ao.WorkingSocket = SocketController.mSock;
            string ipad = Get_MyIP();
            byte[] bDts = Encoding.UTF8.GetBytes("<SOF>" + '/' + ipad);
            ao.WorkingSocket.Send(bDts);
            ao.WorkingSocket.BeginReceive(ao.Buffer, 0, ao.BufferSize, 0, sReceive.DataReceived, ao);
        }
        
        public void AcceptCallback(IAsyncResult ar)
        {
            if (!nowListen)
            {
                SocketController.mSock.BeginAccept(AcceptCallback, null);
                return;
            }
            // 클라이언트의 연결 요청을 수락한다.
            Socket client = SocketController.mSock.EndAccept(ar);
            if (!client.Connected)
            {
                return;
            }
            AsyncObject obj = new AsyncObject(4096);
            obj.WorkingSocket = client;
            // 연결된 클라이언트 리스트에 추가해준다.
            connectedClients.Add(client);

            client.BeginReceive(obj.Buffer, 0, 4096, 0, sReceive.DataReceived, obj);
            SocketController.mSock.BeginAccept(AcceptCallback, null);

            nowListen = true;
        }
    }
}
