using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.View;

namespace TelerikWpfApp3.VM
{
    class ChattingRoomManager
    {
        private static ChattingRoomManager instance = null;

        public static ChattingRoomManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ChattingRoomManager();
                }
                return instance;
            }
        }
        private IDictionary<string, Window> chatRoom;

        public void makeChatRoom(string target)
        {
            Window cR = new ChatRoom(target);
            chatRoom.Add(target,cR);
        }

        public bool findChatRoom(string target)
        {
            if (chatRoom.ContainsKey(target))
            {
                return false;
            }
            else return true;
        }

        public void showChatRoom(string target)
        {
            chatRoom[target].Show();
        }
        
        public void closeChatRoom(string target)
        {
            chatRoom[target].Hide();
        }
        public ChattingRoomManager()
        {
            chatRoom = new Dictionary<string, Window>();
        }
        public void closeAllChatRoom() // 로그아웃 할 때 다 꺼주기
        {
            foreach (string name in chatRoom.Keys)
            {
                chatRoom[name].Hide();
            }
        }

    }
}
