using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.M;

namespace TelerikWpfApp3
{
    class FriendsController
    {
        public FriendsController()
        {
            
        }
        public ObservableCollection<FriendsItem> getFriends()
        {
            return ((App)Application.Current).FriendsList;
        }
        public void LoadMyFriends(string myID)
        {
            ((App)Application.Current).SendData("<FLD>", myID);
        }
        public void AddFriend(string user)
        {
            ((App)Application.Current).FriendsList.Add(new FriendsItem(user));
        }
        public void setfriends(string friendId)
        {
            AddFriend(friendId);
        }
    }
}
