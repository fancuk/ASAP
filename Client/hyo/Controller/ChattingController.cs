using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.M;
using TelerikWpfApp3.VM;
using TelerikWpfApp3.VM.DBControl;

namespace TelerikWpfApp3
{
    class ChattingController
    {
        
        public ChattingController()
        {
            
        }
        public void loadAllChat()
        {
            database sqlite = new database();
            sqlite.ReadChat();
        }
        public void AddSQLChat(string target, Chatitem chatitem)
        {
            ((App)Application.Current).
                chatControl.addChat(target, chatitem);
        }
        public void resetSQLChat(string target)
        {
            ((App)Application.Current).
                chatControl.resetChat(target);
        }
        public void setchatting(string Sender, string Receiver, 
            string Time, string Msg)
        {
            database sqlite = new database();
            sqlite.ChattingCreate(Sender, Receiver, Time, Msg);
        }
        public void setChat(string id)
        {
            database sqlite = new database();
            if (((App)Application.Current).
                Chatdict.ContainsKey(id))
            {

            }
            else
            {
                ((App)Application.Current).
                NowChat = sqlite.ChattingRead(id);

                ((App)Application.Current).Chatdict.Add(id, 
                    ((App)Application.Current).NowChat);
            }
        }
        public ObservableCollection<Chatitem> getChat(string target)
        {
            ((App)Application.Current).NowChat =
                ((App)Application.Current).chatControl.loadChat(target);
            return ((App)Application.Current).NowChat;
        }
        public void AddChat(bool type,string text)
        {
            if (type)
            {
                ((App)Application.Current).NowChat.Add
                    (new Chatitem(text, "보냄", DateTime.Now.ToString(), type));
            }
            else{
                ((App)Application.Current).NowChat.Add
                    (new Chatitem(text, "받음", DateTime.Now.ToString(), type));
            }
        }
    }
}
