using System.Collections.Generic;
using System.Collections.ObjectModel;
using TelerikWpfApp3.M;

namespace TelerikWpfApp3.VM
{
    class ChatControl
    {
        IDictionary<string, ObservableCollection<Chatitem>> Chatdict
               = new Dictionary<string, ObservableCollection<Chatitem>>();

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

        public string getLastChatById(string id)
        {
            if (!Chatdict.ContainsKey(id))
            {
                return "아직 메세지가 없습니다.";
            }
            ObservableCollection<Chatitem> tmp = new ObservableCollection<Chatitem>();
            tmp = this.Chatdict[id];
            Chatitem a = tmp[tmp.Count - 1];
            return a.Text;
        }
    }
}
