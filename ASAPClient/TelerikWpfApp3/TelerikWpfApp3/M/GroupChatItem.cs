using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TelerikWpfApp3.M
{
    public class GroupChatItem : INotifyPropertyChanged
    {
        private string text;
        private string user;
        private string time;
        private bool chk;
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
        public bool Chk
        {
            get { return this.chk; }
            set
            {
                this.chk = value;
                OnPropertyChanged("Chk");
            }
        }
        public GroupChatItem(string text, string user, string time, bool chk)
        {
            this.Text = text;
            this.User = user;
            this.Time = time;
            this.Chk = chk;
        }

        // default 생성자 추가
        public GroupChatItem()
        {

        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    
}
