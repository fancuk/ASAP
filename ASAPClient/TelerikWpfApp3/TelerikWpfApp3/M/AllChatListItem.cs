using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.M
{
    public class AllChatListItem: INotifyPropertyChanged
    {
        private string lastMessage;
        private string target;

        public string LastMessage { get => lastMessage; set
            {
                this.lastMessage = value;
                OnPropertyChanged("LastMessage");
            }
        }
        public string Target { get => target; set {
                this.target = value;
                OnPropertyChanged("Target");
            } }

        public AllChatListItem(string target,string lastMessage)
        {
            this.LastMessage = lastMessage;
            this.Target = target;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
