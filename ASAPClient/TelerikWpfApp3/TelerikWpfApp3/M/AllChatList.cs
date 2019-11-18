using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.M
{
    public class AllChatList // 채팅방 리스트(그냥 라스트메시지, 이름만 있는 말 그대로 채팅방)
    {
        private ObservableCollection<AllChatListItem> chattingList;
        public ObservableCollection<AllChatListItem> ChattingList
        {
            get
            {
                return chattingList;
            }

        }
        AllChatListItem ACL = null;
        public AllChatList()
        {
            chattingList = new ObservableCollection<AllChatListItem>();
        }
        public void AddChattingList(string name, string lastmessage)
        {
            ACL = new AllChatListItem(name, lastmessage);
            chattingList.Add(ACL);
        }
        public ObservableCollection<AllChatListItem> getChattingList()
        {
            return chattingList;
        }
    }
}
