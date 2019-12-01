using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.View;

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
        public void makeChatRoom(string groupIndex,string groupName) // 그룹 채팅방 더블 클릭 시
        {
            //Window gCR = new GroupChatRoom(target); 여기다가 groupChatRoom 클래스 넣어야 함.
            //groupChatRoom.Add(target, gCR);
            Window groupChattingRoom = new GroupChatRoom(groupIndex,groupName);
            groupChatRoom.Add(groupIndex, groupChattingRoom);
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
            Window startWindow = TelerikWpfApp3.StartWindow.Instance;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            screenWidth = (screenWidth / 2) - 197;
            if (screenWidth >= startWindow.Left)
            {
                groupChatRoom[groupIndex].Owner = startWindow;
                groupChatRoom[groupIndex].Top = startWindow.Top;
                groupChatRoom[groupIndex].Left = startWindow.Left + 415;
                groupChatRoom[groupIndex].Show();
            }
            else
            {
                groupChatRoom[groupIndex].Owner = startWindow;
                groupChatRoom[groupIndex].Top = startWindow.Top;
                groupChatRoom[groupIndex].Left = startWindow.Left - 355;
                groupChatRoom[groupIndex].Show();
            }
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
