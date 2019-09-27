using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.MainSocket
{
    public class SocketSend
    {
        #region OnSendData
        public void OnSendData(string type, string Texts)
        {
            // 보낼 텍스트
            string tts = Texts.Trim();

            byte[] bDts = null;
            string str = type + '/' + tts + '/';

            bDts = Encoding.UTF8.GetBytes(str);
            SocketController.mSock.Send(bDts);
        }
        #endregion
    }
}
