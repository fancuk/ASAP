﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.M;
using TelerikWpfApp3.View.Alert;

namespace TelerikWpfApp3.Service
{
    public class WindowManager
    {
        public void ShowLoginView()
        {
            Window s = TelerikWpfApp3.StartWindow.Instance;
            Window n = TelerikWpfApp3.viewtest.Instance;
            n.Show();
            s.Hide();
        }
        public void StartMainWindow()
        {
            Window s = TelerikWpfApp3.viewtest.Instance;
            Window m = TelerikWpfApp3.StartWindow.Instance;
            m.Show();
            s.Hide();
        }
        public void RegisterComplete()
        {
            Window Rg = TelerikWpfApp3.Register.Instance;
            Rg.Hide();
        }

        public void ShowMessageToast(Chatitem chatItem)
        {
            Window mt = TelerikWpfApp3.View.Alert.MessageToast.instance;
            MessageToast messageToast = (MessageToast)mt;
            messageToast.getToastInfo(chatItem.User,
                chatItem.Time,chatItem.Text);
            mt.Show();
           
        }

    }
}
