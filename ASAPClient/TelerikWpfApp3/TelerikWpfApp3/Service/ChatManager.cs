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
    public class ChatManager
    {
        public ChatManager()
        {

        }
        NetworkManager networkManager = ((App)Application.Current).networkManager;
        IDictionary<string, ItemsChangeObservableCollection<Chatitem>> Chatdict
               = new Dictionary<string, ItemsChangeObservableCollection<Chatitem>>();
        AllChatList ACL = new AllChatList();
        LocalDAO localDAO = ((App)Application.Current).localDAO;
        private string nowIReading = null; // 내가 누구 꺼 읽고 있는지

        List<string> friendsReading = new List<string>(); // 여러 명이 내 대화를 보고 있을 

        public bool NowIReading(string target)
        {
            if (target == nowIReading) return true;
            else return false;
        }
        public void addChat(string target, Chatitem chatitem)
        {
            if (!this.Chatdict.ContainsKey(target))
            {
                ItemsChangeObservableCollection<Chatitem> inputTmp = new ItemsChangeObservableCollection<Chatitem>();
                this.Chatdict.Add(target, inputTmp);
            }
            this.Chatdict[target].Add(chatitem);
        }

        public void AddFriendsReading(string friend) // 내가 다른 것을 하고 있을 때, 내 친구가 내가 보낸 채팅 읽을 때!
        {                                           // receive에서 chr true 받으면 이거 수행 시켜주면 될 듯
            friendsReading.Add(friend);
            if (Chatdict.ContainsKey(friend))
            {
                ItemsChangeObservableCollection<Chatitem> tmp = Chatdict[friend];
                for(int i = tmp.Count - 1; i >= 0; i--) //채팅목록 밑에서 부터
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
                if (localDAO != null)
                {
                    localDAO.ChangeChatStatus(friend);
                }
                else
                {
                    localDAO = new LocalDAO();
                    localDAO.ChangeChatStatus(friend);
                }
            }
        }
        public void RemoveFriendsReading(string friend) // 친구가 채팅방 나갈 때
        {                                               // receive에서 chr false면 수행
            friendsReading.Remove(friend);
        }
        public int IsFriendReading(string friend) // send 할 때 쓰면 될 듯!!
        {                                          // 만약 true 받으면 친구가 읽고 있다는 뜻
            int isit = 0;                      // 아 존나게 머리 아파질거 같은게 이 메서드랑 remove메서드랑 동시에 실행되면
                                                   // 뭔가 존나게 복잡해질 거 같은 느낌적인 느낌....썅
            foreach(string target in friendsReading)
            {
                if (target == friend)
                {
                    isit = 1;
                }
            }
            return isit;
        }
    
        public void myRead(string friend)
        {
            // 친구 꺼를 읽어야 한다.
            if (Chatdict.ContainsKey(friend))
            {
                int len = Chatdict[friend].Count-1;
                ItemsChangeObservableCollection<Chatitem> tmp = Chatdict[friend];
                for (int i = len; i >= 0; i--) //채팅목록 밑에서 부터
                {
                    //Chatitem ci = tmp[i];
                    if (Chatdict[friend][i].Status == false) // true는 읽음 false는 안읽음
                    {
                        if (!Chatdict[friend][i].User.Equals(networkManager.MyId))
                        {
                            Chatdict[friend][i].Status = true;
                            //ci.Status = true;
                            //tmp[i] = ci;
                        }
                    }
                }
               // Chatdict[friend] = tmp;
                //localDAO.ChangeChatStatus(friend); => 채팅방 목록에서 chatroom 생성이 안되서 주석처리함 (AllChatlistitem에 status 추가해야함)
                // dao의 alter기능 추가
            }
        }

        public void herReadMe(string her)
        {
            // 친구가 내꺼를 읽는다.
            if (Chatdict.ContainsKey(her))
            {
                ItemsChangeObservableCollection<Chatitem> tmp = Chatdict[her];
                for (int i = tmp.Count - 1; i >= 0; i--) //채팅목록 밑에서 부터
                {
                    Chatitem ci = tmp[i];
                    if (ci.Status == false) // true는 읽음 false는 안읽음
                    {
                        if (ci.User.Equals(networkManager.MyId))
                        {
                            ci.Status = true;
                            tmp[i] = ci;
                        }
                    }
                }
                Chatdict[her] = tmp;
                if (localDAO != null)
                {
                    localDAO.ChangeChatStatus(her);
                }
                else
                {
                    localDAO = new LocalDAO();
                    localDAO.ChangeChatStatus(her);
                }
                // dao의 alter기능 추가
            }
        }

        public void addChat(string target)
        {
            if (!this.Chatdict.ContainsKey(target))
            {
                ItemsChangeObservableCollection<Chatitem> inputTmp = new ItemsChangeObservableCollection<Chatitem>();
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

        public ItemsChangeObservableCollection<Chatitem> getChatList(string target)
        {
            return this.Chatdict[target];
        }
        public ItemsChangeObservableCollection<Chatitem> loadChat(string target)
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
                ItemsChangeObservableCollection<Chatitem> tmp = new ItemsChangeObservableCollection<Chatitem>();
                tmp = this.Chatdict[name];
                Chatitem a = tmp[tmp.Count - 1];
                ACL.ChattingList.Add(new AllChatListItem(name, a.Text, a.Time,tmp.Count.ToString()));
            }
        }
        public void removeUnReadCount(string target)
        {
            AllChatListItem temp;
            for (int i = 0; i < ACL.ChattingList.Count; i++)
            {
                temp = ACL.ChattingList[i];
                if (temp.Target == target)
                {
                    ACL.ChattingList[i].UnReadMessageCount = "0";
                }
            }
        }

        public void addChattingList(string name,string lastMessage, string lastTime, bool isImSender) //대화 보낼 때 마다
        {
            AllChatListItem temp;
            bool isit = false;
            // 이미 채팅리스트에 있는지 확인 해줘야 함
            for (int i = 0; i < ACL.ChattingList.Count; i++)
            {
                temp = ACL.ChattingList[i];
                if (temp.Target == name)
                {
                    temp.LastMessage = lastMessage;
                    temp.LastTime = lastTime;
                    ACL.ChattingList[i] = temp;
                    isit = true;
                    if (!isImSender)
                    {
                        int unReadMessageCountInt = Int32.Parse(ACL.ChattingList[i].UnReadMessageCount);
                        unReadMessageCountInt += 1;
                        string unReadMessageCount = unReadMessageCountInt.ToString();
                        ACL.ChattingList[i].UnReadMessageCount = unReadMessageCount;
                    }
                }
            }
            if (ACL.ChattingList.Count == 0)
            {
                if(isImSender)
                ACL.ChattingList.Add(new AllChatListItem(name, lastMessage, lastTime,"0"));
                else
                    ACL.ChattingList.Add(new AllChatListItem(name, lastMessage, lastTime, "1"));
            }
            else
            {
                if(isit == false)
                {
                    if (isImSender)
                        ACL.ChattingList.Add(new AllChatListItem(name, lastMessage, lastTime, "0"));
                    else
                        ACL.ChattingList.Add(new AllChatListItem(name, lastMessage, lastTime, "1"));
                }
            }
            // 정렬 실패..
        }
        public void RemoveLastChat(string lastTime,string friendID) // ASR에서 false받으면
        {
            ItemsChangeObservableCollection<Chatitem> temp = Chatdict[friendID];
            int tempCount = temp.Count;
            int ChatListcount = ACL.ChattingList.Count;
            if (tempCount == 1) // ASAP 메시지가 처음 이라면 (아예 채팅 처음 시작이 ASAP이라면)
            {
                for(int i = 0; i < ChatListcount; i++)
                {
                    if (ACL.ChattingList[i].LastTime == lastTime)
                    {
                        AllChatListItem delete = ACL.ChattingList[i];
                        ACL.ChattingList.Remove(delete);
                    }
                }
                return;
            }
            if (temp[tempCount - 1].Time == lastTime) // 마지막 메시지가 ASAP이라면
            {
                string lastChat = temp[tempCount - 2].Text;
                string _lastTime = temp[tempCount - 2].Time;
                for (int i = 0; i < ChatListcount; i++)
                {
                    if (ACL.ChattingList[i].LastTime == lastTime)
                    {
                        int unReadMessageCountInt = Int32.Parse(ACL.ChattingList[i].UnReadMessageCount);
                        if (unReadMessageCountInt != 0) unReadMessageCountInt -= 1;
                        string unReadMessageCount = unReadMessageCountInt.ToString();
                        AllChatListItem tempList = new AllChatListItem(friendID, lastChat, _lastTime,unReadMessageCount);
                        ACL.ChattingList[i] = tempList;
                    }
                }
            }
        }
        // 정구
        // ASAP 못읽으면 해당 Chat 삭제.
        public void RemoveChat_ASAP(string friendID, string time) 
        {
            if (Chatdict.ContainsKey(friendID))
            {
                if (Chatdict[friendID].Where(i => i.Time == time).Any())
                    Chatdict[friendID].Remove(Chatdict[friendID].Where(i => i.Time == time).First());
                //2019-11-27 오전 12:30:35

                //Chatdict[receiver].Remove(Chatdict[receiver].Where(i => i.Time == time).Single());
            }
        }

        // 정구
        // ASAP 읽었다는 신호를 Sender가 받으면 LocalDB에 저장.
        public void AddChatInLocalDB_ASAP(string friendID, string time)
        {
            if(localDAO == null)
            {
                localDAO = ((App)Application.Current).localDAO;
            }
            if (Chatdict.ContainsKey(friendID))
            {
                object changeChat; // 이거는 그냥 받는 용도
                if (Chatdict[friendID].Where(i => i.Time == time).Any() != true)
                {
                    return;
                }
                changeChat = Chatdict[friendID].Where(i => i.Time == time).First();

                Chatitem Chat = changeChat as Chatitem;
                localDAO.ChattingCreate(networkManager.MyId, friendID, time, Chat.Text, "Send", 1);
            }
        }

        // 정구
        // ASAP - Receiver가 메세지를 받으면 MessageBox Show
        public void AlertReceiveChat_ASAP(string friendID, string time)
        {
            MessageBox.Show(friendID + " 님에게 " + time + " 에 ASAP 메세지가 왔습니다");
        }
        public IDictionary<string, ItemsChangeObservableCollection<Chatitem>> getDict()
        {
            return Chatdict;

        }

        public void setChat(string target, ItemsChangeObservableCollection<Chatitem> chat)
        {
            this.Chatdict[target] = chat;
        }

        public void resetChat(string target)
        {
            this.Chatdict[target].Clear();
        }
        
        public void AboutFocus(bool isit,string target) // 현재 focus 되어 있는지, 서버 업데이트 되면 실행
        {
            string myID = networkManager.MyId;
            string text = myID + "/" + target;
            if(isit == true)
            {
                myRead(target);
                nowIReading = target;
                networkManager.SendData("<CHR>", text + "/true");
            }
            else
            {
                nowIReading = null;
                networkManager.SendData("<CHR>", text + "/false");
            }
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
