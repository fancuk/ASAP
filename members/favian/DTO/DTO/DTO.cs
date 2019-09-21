using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    class DTO
    {
        static void Main(string[] args)
        {
        }
    }
    public class friends
    {
        private string FriendName;
        private string MyName;

        public friends(string friendName, string myName)
        {
            FriendName = friendName;
            MyName = myName;
        }

        public string friendName { get => FriendName; set => FriendName = value; }
        public string myName { get => MyName; set => MyName = value; }
    }
    public class chatting
    {
        private string Msg;
        private string Time;
        private string Receiver;
        private string Sender;

        public chatting(string msg, string time, 
            string receiver, string sender)
        {
            Msg = msg;
            Time = time;
            Receiver = receiver;
            Sender = sender;
        }

        public string msg { get => Msg; set => Msg = value; }
        public string time { get => Time; set => Time = value; }
        public string receiver { get => Receiver; set => Receiver = value; }
        public string sender { get => Sender; set => Sender = value; }
    }
}
