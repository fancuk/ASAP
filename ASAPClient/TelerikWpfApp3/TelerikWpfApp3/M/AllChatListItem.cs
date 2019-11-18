using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.M
{
    public class AllChatListItem
    {
        private string lastMessage;
        private string target;

        public string LastMessage { get => lastMessage; set => lastMessage = value; }
        public string Target { get => target; set => target = value; }

        public AllChatListItem(string target,string lastMessage)
        {
            this.LastMessage = lastMessage;
            this.Target = target;
        }
    }
}
