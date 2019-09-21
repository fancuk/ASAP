using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                this.Chatdict.Add(target,inputTmp);
            }
            this.Chatdict[target].Add(chatitem);
        }

        public ObservableCollection<Chatitem> loadChat(string target)
        {
            return this.Chatdict[target];
        }

        public  void setChat(string target , ObservableCollection<Chatitem> chat)
        {
            this.Chatdict[target] = chat;
        }

        public void resetChat(string target)
        {
            this.Chatdict[target].Clear();
        }
    }
}
