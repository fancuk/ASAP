using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TelerikWpfApp3.M
{
    class ChatListItem : INotifyPropertyChanged
    {
        private string text;
        private string user;
        private string time;
        private bool chk;
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
        public ChatListItem(string user, string text, string time, bool chk)
        {
            this.Text = text;
            this.User = user;
            this.Time = time;
            this.Chk = chk;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
