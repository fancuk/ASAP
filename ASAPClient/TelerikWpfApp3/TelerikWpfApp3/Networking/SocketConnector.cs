using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.Networking.NetworkModel;
using TelerikWpfApp3.Service;

namespace TelerikWpfApp3.Networking
{
    class SocketConnector
    {
        NetworkManager networkManager = ((App)Application.Current).networkManager;

        private Socket nowSock;
        SocketReciver sr = new SocketReciver();
        SyncSocketReceiver ssr = new SyncSocketReceiver();

        public SocketConnector()
        {
            nowSock =networkManager.ProgramSock;
        }
        public bool SocketConnect()
        {
            string address = "52.231.154.88";
           //string address = "54.180.26.230";
           //string address = "127.0.0.1";
           //string address = "203.229.204.23"; // "127.0.0.1" 도 가능
            int port = 11000;
            return BeginConnection(address, port);
        }

        public bool BeginConnection(string address, int port)
        {
            try
            {
                nowSock.Connect(address, port);
                AfterConnection();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server Connect Fail!");
                return false;
            }
        }

        public void AfterConnection()
        {
            AsyncObject ao = new AsyncObject(4096);
            ao.WorkingSocket = nowSock;
            ao.WorkingSocket.BeginReceive(ao.Buffer, 0, ao.BufferSize, 0, sr.DataReceived, ao);
            networkManager.nowConnect = true;
        }
        public void StartReceive()
        {
            AsyncObject ao = new AsyncObject(4096);
            ao.WorkingSocket = nowSock;
            ao.WorkingSocket.Receive(ao.Buffer);
            string text = Encoding.UTF8.GetString(ao.Buffer);
            ssr.syncReceive(text);
        }
    }
