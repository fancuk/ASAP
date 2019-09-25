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

        public FriendsItem(string user)
        {
            User = user;
        }

        public string User { get => _user; set => _user = value; }
    }
}
