using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.M
{
    public class MyInfo
    {
        private string myId;
        private string email;
        private string pw;

        public MyInfo(string myId, string email, string pw)
        {
            MyId = myId;
            Email = email;
            Pw = pw;
        }

        public string MyId { get => myId; set => myId = value; }
        public string Email { get => email; set => email = value; }
        public string Pw { get => pw; set => pw = value; }
    }
}
