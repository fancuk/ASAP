using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TelerikWpfApp3.Service
{
    class GroupChattingRoomManager
    {
        private static GroupChattingRoomManager instance;
        public static GroupChattingRoomManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GroupChattingRoomManager();
                }
                return instance;
            }
        }
        private IDictionary<string, Window> groupChatRoom; // 그룹 인덱스, 윈도우
        private GroupChattingRoomManager()
        {
            groupChatRoom = new Dictionary<string, Window>();
        }
        public void makeChatRoom(string groupIndex)
        {
            //Window gCR = new GroupChatRoom(target); 여기다가 groupChatRoom 클래스 넣어야 함.
            //groupChatRoom.Add(target, gCR);
        }

        public bool findChatRoom(string groupIndex)
        {
            if (groupChatRoom.ContainsKey(groupIndex))
            {
                return false;
            }
            else return true;
        }

        public void showChatRoom(string groupIndex)
        {
            groupChatRoom[groupIndex].Show();
        }

        public void closeChatRoom(string groupIndex)
        {
            groupChatRoom[groupIndex].Hide();
        }
        public void closeAllChatRoom() // 로그아웃 할 때 다 꺼주기
        {
            foreach (string groupIndex in groupChatRoom.Keys)
            {
                groupChatRoom[groupIndex].Hide();
            }
        }
    }
}
