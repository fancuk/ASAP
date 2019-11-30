using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TelerikWpfApp3.M
{
    public class GroupChatListItem: INotifyPropertyChanged
    {
        private string lastMessage;
        private string groupName;
        private string lastTime;
        private string groupidx; 

        public string LastMessage
        {
            get => lastMessage;
            set
            {
                this.lastMessage = value;
                OnPropertyChanged("LastMessage");
            }
        }
        public string LastTime
        {
            get => lastTime;
            set
            {
                this.lastTime = value;
                OnPropertyChanged("LastTime");
            }
        }
        public string GroupIndex
        {
            get => groupidx;
            set
            {
                this.groupidx = value;
                OnPropertyChanged("GroupIndex");
            }
        }

        public string GroupName
        {
            get => groupName;
            set
            {
                this.groupName = value;
                OnPropertyChanged("GroupName");
            }
        }
        
        public GroupChatListItem(string groupidx, string groupName, string lastMessage, string lastTime)
        {
            this.LastMessage = lastMessage;
            this.LastTime = lastTime;
            this.GroupName = groupName;
            this.GroupIndex = groupidx;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
