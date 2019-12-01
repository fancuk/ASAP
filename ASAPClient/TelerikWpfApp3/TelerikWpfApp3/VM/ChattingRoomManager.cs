using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.View;

namespace TelerikWpfApp3.VM
{
    public class ChattingRoomManager
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
            Window startWindow = TelerikWpfApp3.StartWindow.Instance;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            screenWidth = (screenWidth / 2) - 197;
            if (screenWidth >= startWindow.Left)
            {
                chatRoom[target].Owner = startWindow;
                chatRoom[target].Top = startWindow.Top;
                chatRoom[target].Left = startWindow.Left + 415;
                chatRoom[target].Show();
            }
            else
            {
                chatRoom[target].Owner = startWindow;
                chatRoom[target].Top = startWindow.Top;
                chatRoom[target].Left = startWindow.Left - 300;
                chatRoom[target].Show();
            }
            chatRoom[target].Activate();
        }

        public void removeAsapTopBar(string target)
        {
            ChatRoom s = (ChatRoom)chatRoom[target];
            s.closeTopBar();
        }
        public void showAsapTopBar(string target)
        {
            ChatRoom s = (ChatRoom)chatRoom[target];
            s.showTopBar();
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
