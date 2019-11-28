 using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.M;
using TelerikWpfApp3.Networking.NetworkModel;
using TelerikWpfApp3.View.Alert;
using TelerikWpfApp3.LocalDB;
using TelerikWpfApp3.VM;
using TelerikWpfApp3.Service;

namespace TelerikWpfApp3.Networking
{
    class SocketReciver
    {
        NetworkManager networkManager = ((App)Application.Current).networkManager;
        WindowManager windowManager = ((App)Application.Current).windowManager;
        ChatManager chatManager = ((App)Application.Current).chatManager;
        LocalDAO localDAO = ((App)Application.Current).localDAO;
        UserStatusManager userStatusManager = ((App)Application.Current).userStatusManager;
        ASAPManager asapManager = ((App)Application.Current).asapManager;
        GroupMemberListManager groupMemberListManager = ((App)Application.Current).groupMemberListManager;
        GroupChatManager groupChatManager = ((App)Application.Current).groupChatManager;
        private Socket nowSock;
        SocketCloser sc = new SocketCloser();
        public SocketReciver()
        {
            nowSock =networkManager.ProgramSock;
        }

        #region DataReceived
        public void DataReceived(IAsyncResult ar)
        {
            AsyncObject obj = (AsyncObject)ar.AsyncState;
            try
            {
                if (networkManager.nowConnect == false)
                {
                    return;
                }
                int received = obj.WorkingSocket.EndReceive(ar);
                if (received <= 0)
                {
                    obj.WorkingSocket.Close();
                    return;
                }
                // UTF8 인코더를 사용하여 바이트 배열을 문자열로 변환한다.
                string text = Encoding.UTF8.GetString(obj.Buffer);
                string[] tokens = text.Split('/');
                string tag = tokens[0];
                if (tokens.Length == 1) return;
                if (tag.Equals("<FRR>"))
                {
                    if (tokens[1] == "true")
                    {
                        string target = tokens[2];
                        DispatchService.Invoke(() =>
                        {
                            //((App)Application.Current).AddFriend(target, "true"); 다민
                            FriendsUserControlViewModel.Instance.AddFriend(target, "true"); //다민
                        });
                        MessageBox.Show("친구 추가 되었습니다!");
                    }
                    else
                    {
                        MessageBox.Show("존재하지 않는 ID입니다.");
                    }
                }
                /*else if (tag.Equals("<FRR>")) // 친구추가
                {
                    if (MessageBoxResult.Yes == //친구 요청 받으면
                    MessageBox.Show(tokens[1] + tokens[3], "친구 요청", MessageBoxButton.YesNo))
                    {
                        OnSendData("<FRA>", tokens[1] + "/" + tokens[2]+ "/Yes/");
                    }
                    else
                    {
                        OnSendData("<FRA>", tokens[1] + "/" + tokens[2] + "/No/");
                    }
                    DispatchService.Invoke(() =>
                    {
                        ((App)Application.Current).StartMainWindow();
                    });
                }
                else if (tag.Equals("<FRA>")) // 친구 feedback
                {
                    MessageBox.Show(tokens[2] + tokens[3]);
                    DispatchService.Invoke(() =>
                    {
                        ((App)Application.Current).StartMainWindow();
                    });
                }
                */
                else if (tag.Equals("<LGS>"))
                {
                    string FriendID = tokens[1];
                    string status = tokens[2];
                    DispatchService.Invoke(() =>
                    {
                        FriendsUserControlViewModel.Instance.ChangeStatus(FriendID, status);//다민
                    });
                }
                else if (tag.Equals("<MSG>")) // 메세지
                {
                    Chatitem tmp = new Chatitem();
                    tmp.User = tokens[1];
                    tmp.Time = tokens[3];
                    tmp.Text = tokens[4];
                    if (tokens[5].Equals("true"))
                    {
                        tmp.Asap = true;

                    }
                    else
                    {
                        tmp.Asap = false;
                    }
                    int isitfocus = chatManager.IsFriendReading(tmp.User);
                    if (isitfocus == 1)
                    {
                        tmp.Status = true;
                    }
                    else
                    {
                        tmp.Status = false;
                    }
                    // 여기 이제 수정 필요!!
                    localDAO.ChattingCreate(tokens[1], tokens[2], tokens[3], tokens[4], "Receive", isitfocus); //2019-11-22
                    // sender receiver time msg type status
                    //localDAO.ChattingCreate(tokens[1], tokens[2], tokens[3], tokens[4], "Receive");
                    DispatchService.Invoke(() =>
                    {
                       chatManager.addChat(tmp.User, tmp);
                        chatManager.addChattingList(tokens[1], tokens[4],tokens[3]);
                        Window msgWindow = MessageToast.instance;
                        MessageToast.instance.getToastInfo(tokens[1], tokens[3], tokens[4]);
                        msgWindow.Show();
                    });
                    // if (!((App)Application.Current).mqState)
                    //{
                    //    ((App)Application.Current).resetSQLChat(tmp.User);
                    //}
                    //DispatchService.Invoke(() =>
                    //{
                    //   // ((App)Application.Current).LoadMSGAlert();
                    //});
                }
                else if (tag.Equals("<FIN>"))
                {
                    if (networkManager.nowConnect == true)
                    {
                        sc.closeSock();
                    }
                }
                else if (tag.Equals("<CHR>")) // 서버 업데이트 후에
                {
                    string friendID = tokens[1];
                    string isitFocus = tokens[2];
                    if(isitFocus == "true")
                    {
                        DispatchService.Invoke(() =>
                        {
                            chatManager.herReadMe(friendID);
                        });
                    }
                    else
                    {
                        DispatchService.Invoke(() =>
                        {
                            chatManager.RemoveFriendsReading(friendID);
                        });
                    }
                }
                else if (tag.Equals("<ASG>"))
                {
                    string friendID = tokens[1];
                    string time = tokens[2];
                    chatManager.AlertReceiveChat_ASAP(friendID, time);
                }
                else if (tag.Equals("<ASR>"))
                {
                    string friendID = tokens[1];
                    string time = tokens[2];
                    string isit = tokens[3];
                    if (isit.Equals("true"))
                    {
                        // sqlite에 저장해라!
                        chatManager.AddChatInLocalDB_ASAP(friendID, time);
                    }
                    else
                    {
                        asapManager.RemoveLastChat(time);
                        chatManager.RemoveChat_ASAP(friendID, time);
                    }
                }
                else if (tag.Equals("<MKG>"))
                {
                    if (tokens[1].Equals("false"))
                    {
                        MessageBox.Show("그룹 채팅방 만들기 실패 ㅠ");
                    }
                    else
                    {
                        string maker = tokens[1];
                        string groupName = tokens[2];
                        string groupIdx = tokens[3];
                        int memberCount = int.Parse(tokens[4]);
                        List<string> groupMemberList = new List<string>(memberCount);
                        string[] nameSlice = tokens[5].Split('^');
                        int length = nameSlice.Length;
                        string time = DateTime.Now.ToString();
                        groupMemberList.Add(maker);
                        for (int i = 0; i < length; i++)
                        {
                            groupMemberList.Add(nameSlice[i]);
                        }
                        groupMemberListManager.AddGroupMemberList(groupIdx, groupMemberList);
                        GroupChattingRoomManager.Instance.makeChatRoom(groupIdx);
                        if (maker == networkManager.MyId) // 만든 사람이 나라면
                        {
                            string plain = maker + "님이 채팅방을 만드셨습니다.";
                            networkManager.SendData
                                ("<GSG>", maker + "/" + groupIdx + "/" + plain + "/" + time);
                            groupChatManager.addChattingList(maker, groupIdx, plain, time); // 자신은 gsg를  안받기 때문에 추가해주기
                            groupChatManager.addChat(groupIdx, new GroupChatItem(plain, maker, time, true)); // check가 true면 내가 보낸건가?
                            GroupChattingRoomManager.Instance.showChatRoom(groupIdx);
                        }
                        //localDAO.GroupInfoCreate(groupIdx, groupName, maker + "^" + tokens[5]); // DAO에 넣어주는거 추가했습니다 BY 정구
                        // 그룹 채팅방 만들어주고 거기다가 maker가 만들었습니다 라고 메시지로 띄우기 (maker가 보낸 것 처럼)

                    }
                }
                else if (tag.Equals("<GSG>"))
                {
                    string sender = tokens[1];
                    string gIdx = tokens[2];
                    string plain = tokens[3];
                    string time = tokens[4];
                    // dao에 넣어주고
                    // 그룹 채팅방에 메시지 보내주기
                    groupChatManager.addChattingList(gIdx, null, plain, time); // null 값에 groupname을 받아오자
                                                                               //  groupchatmanager에게 gidx를 주면 받을 수 있게 구현하자 
                    groupChatManager.addChat(gIdx, new GroupChatItem(plain, sender, time, false));
                }
                // 텍스트박스에 추가해준다.
                // 비동기식으로 작업하기 때문에 폼의 UI 스레드에서 작업을 해줘야 한다.
                // 따라서 대리자를 통해 처리한다.

                //^^^^^^^^
                // 클라이언트에선 데이터를 전달해줄 필요가 없으므로 바로 수신 대기한다.
                // 데이터를 받은 후엔 다시 버퍼를 비워주고 같은 방법으로 수신을 대기한다.
                if (networkManager.nowConnect == true) //예외 처리 obj beginreceive
                {
                    obj.ClearBuffer();

                    // 수신 대기
                    obj.WorkingSocket.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
                }
            }
            catch (Exception e)
            {
                if (networkManager.nowConnect == true)
                {
                    sc.closeSock();
                }
                MessageBox.Show("서버와의 연결 오류!");
                MessageBox.Show(e.ToString());

                return;
            }
        }
        #endregion
    }
}
