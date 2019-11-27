using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelerikWpfApp3.M;
using TelerikWpfApp3.LocalDB;
using TelerikWpfApp3.VM;
using System.Windows;
using TelerikWpfApp3.Collection;

namespace TelerikWpfApp3.Service
{
    public class GroupChatManager
    {

        public GroupChatManager()
        {
          localDAO = ((App)Application.Current).localDAO;
        }

        LocalDAO localDAO;
        NetworkManager networkManager = ((App)Application.Current).networkManager;
        IDictionary<string, ItemsChangeObservableCollection<GroupChatItem>> GroupChatDict
        = new Dictionary<string, ItemsChangeObservableCollection<GroupChatItem>>();
        //dictionary = groupindex, groupchatitem

        GroupChatList GCL = new GroupChatList();

        public void addChat(string groupidx, GroupChatItem groupChatItem)
        {//새로운 채팅 추가
          
            if (!this.GroupChatDict.ContainsKey(groupidx))
            {
                ItemsChangeObservableCollection<GroupChatItem> inputTmp = new ItemsChangeObservableCollection<GroupChatItem>();
                this.GroupChatDict.Add(groupidx, inputTmp);
            }
            this.GroupChatDict[groupidx].Add(groupChatItem);
        }

        public void addChattingList(string groupidx, string groupName,string lastMessage, string lastTime)
        {
            GroupChatListItem tmp;
            bool isit = false;
            //여기 수정해야함
            for(int i=0;i< GCL.GroupChattingList.Count;i++)
            {
                tmp = GCL.GroupChattingList[i];
                
            }
        }

        public void addChat(string groupidx)
        {
            if (!this.GroupChatDict.ContainsKey(groupidx))
            {
                ItemsChangeObservableCollection<GroupChatItem> inputTmp = new ItemsChangeObservableCollection<GroupChatItem>();
                this.GroupChatDict.Add(groupidx, inputTmp);
            }
        }

        public ItemsChangeObservableCollection<GroupChatItem> loadChat(string groupidx)//해당 groupidx 채팅방 로드
        {
            if(GroupChatDict.ContainsKey(groupidx))
            {
                return this.GroupChatDict[groupidx];
            }
            else
            {
                addChat(groupidx);
                return this.GroupChatDict[groupidx];
            }
        }

        public ItemsChangeObservableCollection<GroupChatItem> getChatList(string groupidx)
        {
            return this.GroupChatDict[groupidx];
        }

    }                                                                                                                            
}
