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

namespace TelerikWpfApp3.Service
{
    public class ChatManager
    {
        public ChatManager()
        {

        }
        NetworkManager networkManager = ((App)Application.Current).networkManager;
        IDictionary<string, ObservableCollection<Chatitem>> Chatdict
               = new Dictionary<string, ObservableCollection<Chatitem>>();
        AllChatList ACL = new AllChatList();

        List<string> friendsReading = new List<string>(); // 여러 명이 내 대화를 보고 있을 경우
        public void addChat(string target, Chatitem chatitem)
        {
            if (!this.Chatdict.ContainsKey(target))
            {
                ObservableCollection<Chatitem> inputTmp = new ObservableCollection<Chatitem>();
                this.Chatdict.Add(target, inputTmp);
            }
            this.Chatdict[target].Add(chatitem);
        }

        public void AddFriendsReading(string friend) // 내가 다른 것을 하고 있을 때, 내 친구가 내가 보낸 채팅 읽을 때!
        {                                           // receive에서 chr true 받으면 이거 수행 시켜주면 될 듯
            friendsReading.Add(friend);
            if (Chatdict.ContainsKey(friend))
            {
                ObservableCollection<Chatitem> tmp = Chatdict[friend];
                for(int i = tmp.Count - 1; i >= 0; i--)
                {
                    Chatitem ci = tmp[i];
                    if (ci.Status == false) // true는 읽음 false는 안읽음
                    {
                        ci.Status = true;
                        tmp[i] = ci;
                    }
                    else
                    {
                        break; // 그 전의 메시지는 이미 읽었을 것임.
                    }
                }
                Chatdict[friend] = tmp;
            }
        }
        public void RemoveFriendsReading(string friend) // 친구가 채팅방 나갈 때
        {                                               // receive에서 chr false면 수행
            friendsReading.Remove(friend);
        }
        public bool IsFriendReading(string friend) // send 할 때 쓰면 될 듯!!
        {                                          // 만약 true 받으면 친구가 읽고 있다는 뜻
            bool isit = false;                      // 아 존나게 머리 아파질거 같은게 이 메서드랑 remove메서드랑 동시에 실행되면
                                                   // 뭔가 존나게 복잡해질 거 같은 느낌적인 느낌....썅
            foreach(string target in friendsReading)
            {
                if (target == friend)
                {
                    isit = true;
                }
            }
            return isit;
        }
        public void addChat(string target)
        {
            if (!this.Chatdict.ContainsKey(target))
            {
                ObservableCollection<Chatitem> inputTmp = new ObservableCollection<Chatitem>();
                this.Chatdict.Add(target, inputTmp);
            }
        }

        public void FriendRead(string friend)
        {

        }

        public ObservableCollection<AllChatListItem> getChattingList()
        {
            return ACL.ChattingList;
        }
        public ObservableCollection<Chatitem> loadChat(string target)
        {
            if (Chatdict.ContainsKey(target))
            {
                return this.Chatdict[target];
            }
            else
            {
                addChat(target);
                return this.Chatdict[target];
            }
        }

        public void setChattingList() // 처음 세팅만
        {
            foreach(string name in Chatdict.Keys)
            {
                ObservableCollection<Chatitem> tmp = new ObservableCollection<Chatitem>();
                tmp = this.Chatdict[name];
                Chatitem a = tmp[tmp.Count - 1];
                ACL.ChattingList.Add(new AllChatListItem(name, a.Text));
            }
        }
        public void addChattingList(string name,string lastMessage) //대화 보낼 때 마다
        {
            AllChatListItem temp;
            bool isit = false;
            // 이미 채팅리스트에 있는지 확인 해줘야 함
            for(int i = 0; i < ACL.ChattingList.Count; i++)
            {
                temp = ACL.ChattingList[i];
                if (temp.Target == name)
                {
                    temp.LastMessage = lastMessage;
                    ACL.ChattingList[i] = temp;
                    isit = true;
                }
            }
            if (ACL.ChattingList.Count == 0)
            {
                ACL.ChattingList.Add(new AllChatListItem(name, lastMessage));
            }
            else
            {
                if(isit == false)
                {
                    ACL.ChattingList.Add(new AllChatListItem(name, lastMessage));
                }
            }
        }
        public IDictionary<string, ObservableCollection<Chatitem>> getDict()
        {
            return Chatdict;

        }

        public void setChat(string target, ObservableCollection<Chatitem> chat)
        {
            this.Chatdict[target] = chat;
        }

        public void resetChat(string target)
        {
            this.Chatdict[target].Clear();
        }
        
        public void AboutFocus(bool isit,string target) // 현재 focus 되어 있는지, 서버 업데이트 되면 실행
        {
            /*string myID = networkManager.MyId;
            string text = myID + "/" + target;
            if(isit == true)
            {
                networkManager.SendData("<CHR>", text + "/true");
            }
            else
            {
                networkManager.SendData("<CHR>", text + "/false");
            }*/
        }

        /*public string getLastChatById(string id)
        {
            if (!Chatdict.ContainsKey(id))
            {
                return "아직 메세지가 없습니다.";
            }
            ObservableCollection<Chatitem> tmp = new ObservableCollection<Chatitem>();
            tmp = this.Chatdict[id];
            Chatitem a = tmp[tmp.Count - 1];
            return a.Text;
        }다민*/
    }
}
