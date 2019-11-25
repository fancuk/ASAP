using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.M
{
    public class Chatitem : INotifyPropertyChanged
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
        public bool Chk
        {
            get { return this.chk; }
            set
            {
                this.chk = value;
                OnPropertyChanged("Chk");
            }
        }

        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                OnPropertyChanged("Text");
            }
        }
        public string User
        {
            get { return this.user; }
            set
            {
                this.user = value;
                OnPropertyChanged("User");
            }
        }
        public string Time
        {
            get { return this.time; }
            set
            {
                this.time = value;
                OnPropertyChanged("Time");
            }
        }
        public bool Asap
        {
            get { return this.asap; }
            set
            {
                this.asap = value;
                OnPropertyChanged("Asap");
            }
        }
        public bool Status
        {
            get { return this.status; }
            set
            {
                this.status = value;
                OnPropertyChanged("Status");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
