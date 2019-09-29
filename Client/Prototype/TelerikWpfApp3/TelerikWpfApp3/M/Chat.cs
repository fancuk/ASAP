using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.M
{
    public class Chat
    {
        private string _id;
        public string id { get { return this._id; } set { this.id = value; } }
        public ObservableCollection<ClientItem> MessageList;
        public Chat()
        {
               
        }
    }
}
