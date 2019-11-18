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
        IDictionary<string, ObservableCollection<Chatitem>> Chatdict
               = new Dictionary<string, ObservableCollection<Chatitem>>();
        AllChatList ACL = new AllChatList();
        AllChatListItem temp;

        public void addChat(string target, Chatitem chatitem)
        {
            if (!this.Chatdict.ContainsKey(target))
            {
                ObservableCollection<Chatitem> inputTmp = new ObservableCollection<Chatitem>();
                this.Chatdict.Add(target, inputTmp);
            }
            this.Chatdict[target].Add(chatitem);
        }

        public void addChat(string target)
        {
            if (!this.Chatdict.ContainsKey(target))
            {
                ObservableCollection<Chatitem> inputTmp = new ObservableCollection<Chatitem>();
                this.Chatdict.Add(target, inputTmp);
            }
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
