using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TelerikWpfApp3.M;
using TelerikWpfApp3.Networking;
using TelerikWpfApp3.Utility;
using TelerikWpfApp3.VM;

namespace TelerikWpfApp3.Service
{
    public class NetworkManager
    {
        // * 이 클래스가 이제 App.xaml.cs 를 대체합니다.
        // 모든 의존은 이 클래스에서 집중적으로 일어나야 하며, 
        // 점차적으로 App.xaml.cs의 메소드를 제거하는 작업을 통해
        // App.xaml.cs를 공백화 시킬것입니다.
        // 추가적으로 작성할 Dependency, Property, Operation은 각 Region에 맞게 추가 해주세요
        // 작성자 이정환


        #region Dependency
        SocketConnector socketConnector;
        SocketSender socketSender;
        SocketCloser socketCloser;
        #endregion

        #region Status Check
        public bool nowConnect = false;
        public string nowConnectStatus = "false";
        public string MyId { get; set; }
        #endregion

        #region Collections
        IDictionary<string, ObservableCollection<Chatitem>> Chatdict = new Dictionary<string, ObservableCollection<Chatitem>>();
        public static ObservableCollection<Chatitem> NowChat = new ObservableCollection<Chatitem>();
        #endregion

        #region Socket
        public Socket ProgramSock { get; set; }
        #endregion

        #region Socket Networking
        public bool StartSocket()
        {
            makeSocket();
            return socketConnector.SocketConnect();
        }
        public void makeSocket()
        {
            SocketMaker sm = new SocketMaker();
            ProgramSock = sm.makeSock(ProgramSock);
            InitSocketInstance();
        }
        public void InitSocketInstance()
        {
            socketConnector = new SocketConnector();
            socketSender = new SocketSender();
            socketCloser = new SocketCloser();
        }
        public void SendData(string type, string text)
        {
            if (nowConnect == false) StartSocket();
            socketSender.OnSendData(type, text);
        }
        public void CloseSocket()
        {
            socketCloser.closeSock();
        }
        #endregion
    }
}
