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


        #region 그룹 채팅 리스트 목록
        public void addChattingList(string groupidx, string groupName,string lastMessage, string lastTime)
        {                               // Send 할 때도 호출 해줘야 함 GroupChatRoomViewModel.cs -> sendmessage
            GroupChatListItem tmp;
            bool isit = false;
            //여기 수정해야함
            for(int i=0;i< GCL.GroupChattingList.Count;i++)
            {
                tmp = GCL.GroupChattingList[i];
                if (tmp.GroupIndex == groupidx)
                {
                    tmp.LastMessage = lastMessage;
                    tmp.LastTime = lastTime;
                    GCL.GroupChattingList[i] = tmp;
                    isit = true;
                }
            }
            if (GCL.GroupChattingList.Count == 0)
            {
                GCL.GroupChattingList.Add(new GroupChatListItem(groupName, lastMessage, lastTime));
            }
            else
            {
                if (isit == false)
                {
                    GCL.GroupChattingList.Add(new GroupChatListItem(groupName, lastMessage, lastTime));
                }
            }
        }
        public void setChattingList() // 처음 세팅만
        {
            foreach (string gIdx in GroupChatDict.Keys)
            {
                ItemsChangeObservableCollection<GroupChatItem> tmp = new ItemsChangeObservableCollection<GroupChatItem>();
                tmp = this.GroupChatDict[gIdx];
                GroupChatItem a = tmp[tmp.Count - 1];
                //GCL.GroupChattingList.Add(new GroupChatListItem(a.GroupName,a.Text,a.Text); GroupChatItem 수정 후에 주석 풀기
            }
        }
        public ItemsChangeObservableCollection<GroupChatListItem> getGroupChattingList()
        {
            return GCL.GroupChattingList; // 그룹 채팅 목록 여기다가 바인딩
        }
        #endregion
        #region 그룹 채팅 관련
        public void makeGroupChat(string groupName, string groupMembers)
        {
            string groupMaker = networkManager.MyId;
            networkManager.SendData("<MKG>", groupMaker + "/" + groupName + "/" + groupMembers);
        }
        public void addChat(string groupidx, GroupChatItem groupChatItem)
        {//새로운 채팅 추가

            if (!this.GroupChatDict.ContainsKey(groupidx))
            {
                ItemsChangeObservableCollection<GroupChatItem> inputTmp = new ItemsChangeObservableCollection<GroupChatItem>();
                this.GroupChatDict.Add(groupidx, inputTmp);
            }
            this.GroupChatDict[groupidx].Add(groupChatItem);
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
        #endregion
    }
}
