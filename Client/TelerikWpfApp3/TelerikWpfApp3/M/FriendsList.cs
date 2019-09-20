using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TelerikWpfApp3
{
    class FriendsList: INotifyPropertyChanged
    {
        private List<string> friends = new List<string>();
        public FriendsList()
        {

        }
        public List<string> getFriends()
        {
            return friends;
        }
        public void setFriends(string friend)
        {
            friends.Add(friend);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
