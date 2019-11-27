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
using TelerikWpfApp3.LocalDB;
using TelerikWpfApp3.Service;

namespace TelerikWpfApp3
{
    public partial class App : Application
    {
        public NetworkManager networkManager;
        public LocalDAO localDAO;
        public WindowManager windowManager;
        public ChatManager chatManager;
        public DabbingPreventor dabbingPreventor;
        public UserStatusManager userStatusManager;
        public GroupChatManager groupChatManager;
        public App()
        {
             networkManager = new NetworkManager();
            chatManager = new ChatManager();
            localDAO = new LocalDAO();
             windowManager = new WindowManager();
             dabbingPreventor = new DabbingPreventor();
            userStatusManager = new UserStatusManager();
            groupChatManager = new GroupChatManager();
            Startup += App_Startup;
            InitializeComponent();
        }
        private void App_Startup(object sender, StartupEventArgs e)
        {
            TelerikWpfApp3.viewtest.Instance.Show();
        }
    }
}
