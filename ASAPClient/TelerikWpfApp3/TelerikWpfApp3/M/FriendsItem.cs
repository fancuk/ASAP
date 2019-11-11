using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.M
{
    public  class FriendsItem
    {
        private string _user;
        private string _status;
        private string _lastMesseage;


        public FriendsItem(string user, string lastMesseage, string _status)
        {
            User = user;
            LastMesseage = lastMesseage;
            this._status = _status;
        }

        public string User { get => _user; set => _user = value; }
        public string LastMesseage { get => _lastMesseage; set => _lastMesseage = value; }

        public string Status { get => _status; set => _status = value; }
    }
}
