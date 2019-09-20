using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.M
{
    public class ClientItem
    {
        private string user;
        private string status;
        private bool chk;

        public ClientItem(string user, string status, bool chk)
        {
            this.User = user;
            this.Status = status;
            this.Chk = chk;
        }

        public string User { get => user; set => user = value; }
        public string Status { get => status; set => status = value; }
        public bool Chk { get => chk; set => chk = value; }
    }
}
