using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelerikWpfApp3.Collection;

namespace TelerikWpfApp3.M
{
    public class AllChatList // 채팅방 리스트(그냥 라스트메시지, 이름만 있는 말 그대로 채팅방)
    {
        private ItemsChangeObservableCollection<AllChatListItem> chattingList;
        public ItemsChangeObservableCollection<AllChatListItem> ChattingList
        {
            get
            {
                return chattingList;
            }
            set
            {
                this.chattingList = value;
            }
        }
        AllChatListItem ACL = null;
        public AllChatList()
        {
            chattingList = new ItemsChangeObservableCollection<AllChatListItem>();
        }
        public ItemsChangeObservableCollection<AllChatListItem> getChattingList()
        {
            return chattingList;
        }
    }
}
