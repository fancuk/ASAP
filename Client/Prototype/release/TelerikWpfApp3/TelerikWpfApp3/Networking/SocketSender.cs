﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.M;
using TelerikWpfApp3.Networking.NetworkModel;

namespace TelerikWpfApp3.Networking
{
    class SocketSender
    {
        private Socket nowSock;
        public SocketSender()
        {
            nowSock = ((App)Application.Current).ProgramSock;
        }
        public void OnSendData(string type, string Texts)
        {
            // 보낼 텍스트
            string tts = Texts.Trim();

            byte[] bDts = null;
            string str = type + '/' + tts + '/';

            bDts = Encoding.UTF8.GetBytes(str);
            nowSock.Send(bDts);
        }
    }
}
