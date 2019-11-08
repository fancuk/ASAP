using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.VM.DBCmember
{
    class DTO
    {
        
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
