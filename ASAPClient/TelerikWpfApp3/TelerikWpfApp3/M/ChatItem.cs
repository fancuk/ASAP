using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.M
{
    public class Chatitem
    {
        private string text;
        private string user;
        private string time;
        private bool chk;
        private bool asap;
        private bool status;
        public Chatitem(string text, string user, string time, bool chk, bool asap)
        {
            this.Text = text;
            this.User = user;
            this.Time = time;
            this.Chk = chk;
            this.Asap = asap;
        }
        public Chatitem()
        {

        }
        public string Text { get => text; set => text = value; }
        public string User { get => user; set => user = value; }
        public string Time { get => time; set => time = value; }
        public bool Chk { get => chk; set => chk = value; }
        public bool Asap { get => asap; set => asap = value; }
        public bool Status { get => status; set => status = value; }
    }
}
