using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.M
{
    public class AllChatList
    {
        private string _target;
        public string target
        {
            get { return this._target; }
            set { this._target = value; }
        }
        public ObservableCollection<Chatitem> MessageList;
    }
}
