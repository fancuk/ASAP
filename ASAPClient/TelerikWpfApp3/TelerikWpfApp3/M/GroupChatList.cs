using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelerikWpfApp3.Collection;

namespace TelerikWpfApp3.M
{
    public class GroupChatList
    {
        private ItemsChangeObservableCollection<GroupChatListItem> groupChattingList;
        public ItemsChangeObservableCollection<GroupChatListItem> GroupChattingList
        {
            get => groupChattingList;
            set
            {
                this.groupChattingList = value;
            }
        }

        public GroupChatList()
        {
            groupChattingList = new ItemsChangeObservableCollection<GroupChatListItem>();
        }
    }
}
