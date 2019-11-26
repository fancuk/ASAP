using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.M;
using TelerikWpfApp3.VM;

namespace TelerikWpfApp3.Service
{
    public class ASAPManager
    {
        private List<string> sentASAPList = new List<string>();
        ChatManager chatManager = ((App)Application.Current).chatManager;
        public ASAPManager() // Application에서만 생성!!!!!
        {
        }
        #region ASAP 보낸거 확인,추가,삭제 기능
        public bool ASAP_SentCheck(string target) // 내가 보낸 ASAP 친구들 조회
        {
            int count = sentASAPList.Count;
            for(int i = 0; i < count; i++)
            {
                if (sentASAPList[i] == target)
                {
                    return false; // 이미 보냈다 보내지 마라.
                }
            }
            return true;
        }
        public void ASAP_PlusSentList(string target) // ASAP 보내기 성공 했다면 리스트에 추가
        {
            sentASAPList.Add(target);
            // Send하면 추가해주기
        }
        public void ASAP_RemoveSentList(string target) // 확인 응답이 왔다면 리스트에서 삭제
        {
            sentASAPList.Remove(target);
            // ASR fasle든 true든 바꿔줘야 함
        }
        #endregion
        #region 상대 온라인인지 확인하고 ASAP 보내기
        public bool IsSheLogin(string target)
        {
            bool isShe = FriendsUserControlViewModel.Instance.OnlineCheck(target);
            return isShe;
        }
        #endregion
        #region asr false받으면 삭제해주기
        public void RemoveLastChat(string lastTime)
        {
            chatManager.RemoveLastChat(lastTime);
        }
        #endregion
    }
}
