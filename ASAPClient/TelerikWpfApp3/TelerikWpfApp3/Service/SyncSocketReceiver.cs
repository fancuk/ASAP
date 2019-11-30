using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.LocalDB;
using TelerikWpfApp3.M;
using TelerikWpfApp3.Networking;
using TelerikWpfApp3.Networking.NetworkModel;
using TelerikWpfApp3.View.Alert;
using TelerikWpfApp3.VM;

namespace TelerikWpfApp3.Service
{
    class SyncSocketReceiver
    {
        private Socket nowSock;
        NetworkManager networkManager = ((App)Application.Current).networkManager;
        ChatManager chatManager = ((App)Application.Current).chatManager;
        WindowManager windowManager = ((App)Application.Current).windowManager;
        UserStatusManager userStatusManager = ((App)Application.Current).userStatusManager;
        LocalDAO localDAO = ((App)Application.Current).localDAO;
        GroupChatManager groupChatManager = ((App)Application.Current).groupChatManager;
        GroupMemberListManager groupMemberListManager = ((App)Application.Current).groupMemberListManager;
        SocketCloser sc = new SocketCloser();
        SocketReciver sr = new SocketReciver();
        public void syncReceive(string text)
        {
            AsyncObject ao = new AsyncObject(4096);
            ao.WorkingSocket = nowSock;
            int isit = 1; // 1 = 동기, 2 = 비동기, 3 = 로그인 성공 후에 n개 받기 위한 동기 + 비동기
            try
            {
                string[] tokens = text.Split('/');
                if (networkManager.nowConnect == false)
                {
                    return;
                }
                string tag = tokens[0];
                if (tag.Equals("<LOG>")) // 로그인
                {
                    string flag = tokens[1];
                    if (flag.Equals("true"))
                    {
                        Properties.Settings.Default.loginOK = true; // 로그인 성공여부
                        if (Properties.Settings.Default.idSaveCheck == false)
                        {
                            Properties.Settings.Default.loginIdSave = networkManager.MyId; //id 저장
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            Properties.Settings.Default.loginIdSave = "";
                            Properties.Settings.Default.Save();
                        }
                        DispatchService.Invoke(() =>
                        {
                            windowManager.StartMainWindow();
                        });
                        string myId = networkManager.MyId;
                        isit = 3;
                        networkManager.SendData("<MSQ>", myId);
                    }
                    else
                    {
                        MessageBox.Show("Login Failed.....TT");
                        Properties.Settings.Default.loginOK = false;
                        networkManager.CloseSocket();
                    }
                }
                else if (tag.Equals("<REG>")) // 회원가입
                {
                    string flag = tokens[1];
                    if (flag.Equals("true"))
                    {
                        DispatchService.Invoke(() =>
                        {
                            MessageBox.Show("회원가입 완료!!");
                            windowManager.RegisterComplete();
                        });
                    }

                    else
                    {
                        MessageBox.Show("Register Failed.....TT");
                        Window x = new FalseMsgBox("Fail!");
                        DispatchService.Invoke(() => //너무 많은 UI 어쩌구저쩌구 SPA? STA 나와서 invoke 처리 2019-09-19 다민
                        {
                            x.Show();
                        });
                    }
                    networkManager.CloseSocket();
                }
                else if (tag.Equals("<ICF>")) // ID 체크
                {
                    string flag = tokens[1];
                    if (flag.Equals("true"))
                    {
                        MessageBox.Show("ID Check Sucess! in view");
                        DispatchService.Invoke(() =>
                        {
                            RegisterViewModel a = TelerikWpfApp3.Register.Instance.DataContext as RegisterViewModel;
                            a.nameChk = "V";
                            TelerikWpfApp3.Register.Instance.DataContext = a;
                            userStatusManager.Idchk = (true);
                        });
                    }
                    else
                    {
                        MessageBox.Show("ID Check Failed.....TT");
                        userStatusManager.Idchk = (false);
                        DispatchService.Invoke(() =>
                        {
                            RegisterViewModel a = TelerikWpfApp3.Register.Instance.DataContext as RegisterViewModel;
                            a.nameChk = "X";
                            TelerikWpfApp3.Register.Instance.DataContext = a;
                            userStatusManager.Idchk = (true);
                        });
                    }
                    networkManager.CloseSocket();
                }
                else if (tag.Equals("<MSQ>"))
                {

                    int count = Int32.Parse(tokens[1]);
                    int idx = 2;
                    for (int i = 0; i < count; i++)
                    {
                        string[] token2 = tokens[idx + i].Split(',');
                        string user = token2[0];
                        string msg = token2[1];
                        string time = token2[2];

                        Chatitem tmp = new Chatitem();
                        tmp.User = user;
                        tmp.Time = time;
                        tmp.Text = msg;
                        localDAO.ChattingCreate(user,      //2019-11-22
                            networkManager.MyId, time, msg, "Receive", 0);

                        DispatchService.Invoke(() =>
                        {
                            chatManager.addChat(tmp.User, tmp);
                        });
                    }
                    string myId = networkManager.MyId;
                    networkManager.SendData("<MKQ>", myId);
                    isit = 3;
                }
                else if (tag.Equals("<FIN>"))
                {
                    if (networkManager.nowConnect == true)
                    {
                        sc.closeSock();
                        windowManager.CloseAll();
                    }
                }
                else if (tag.Equals("<FLD>"))
                {
                    int count = Int32.Parse(tokens[1]);
                    int idx = 2;
                    for (int i = 0; i < count; i++)
                    {
                        string[] resToken = tokens[idx + i].Split('^');
                        DispatchService.Invoke(() =>
                        {
                            FriendsUserControlViewModel.Instance.AddFriend(resToken[0], resToken[1]);
                        });
                        isit = 2;
                    }
                    DispatchService.Invoke(() =>
                    {
                        localDAO.ReadChat();
                        localDAO.ReadGroupList();
                    });
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
                        string groupTime = tokens[4];
                        int memberCount = int.Parse(tokens[5]);
                        List<string> groupMemberList = new List<string>(memberCount);
                        string[] nameSlice = tokens[6].Split('^');
                        int length = nameSlice.Length;
                        for (int i = 0; i < length; i++)
                        {
                            groupMemberList.Add(nameSlice[i]);
                        }
                        groupMemberListManager.AddGroupMemberList(groupIdx, groupMemberList);
                        string plain = maker + "님이 채팅방을 만드셨습니다.";
                        DispatchService.Invoke(() =>
                        {
                            groupChatManager.addChattingList(groupIdx, groupName, plain, groupTime);
                            GroupChattingRoomManager.Instance.makeChatRoom(groupIdx, groupName);
                        });
                        if (maker == networkManager.MyId) // 만든 사람이 나라면
                        {
                            DispatchService.Invoke(() =>
                            {
                                groupChatManager.addChat(groupIdx, new GroupChatItem(plain, maker, groupTime, true)); // check가 true면 내가 보낸건가?
                                GroupChattingRoomManager.Instance.showChatRoom(groupIdx);
                            });
                        }
                        else
                        {
                            groupChatManager.addChat(groupIdx, new GroupChatItem(plain, maker, groupTime, false)); // check가 true면 내가 보낸건가?
                        }
                        localDAO.GroupInfoCreate(groupIdx, groupName, maker + "^" + tokens[5]);
                        localDAO.GroupChattingCreate(maker, groupIdx, groupTime, plain);
                        isit = 2;
                    }
                }
                else if (tag.Equals("<MKQ>"))//만든 사람 생각 안해줘도 된다.
                {
                    int groupCount = int.Parse(tokens[1]);
                    for (int i = 0; i < groupCount; i++)
                    {
                        string[] groupInfo = tokens[i + 2].Split('&');
                        string[] groupMemberInfo = groupInfo[5].Split('^');
                        string maker = groupInfo[0];
                        string groupName = groupInfo[1];
                        string gIdx = groupInfo[2];
                        string time = groupInfo[3];
                        int memberCount = int.Parse(groupInfo[4]);
                        List<string> groupMemberList = new List<string>(memberCount);
                        for (int j = 0; j < memberCount; j++)
                        {
                            groupMemberList.Add(groupMemberInfo[j]);
                        }
                        DispatchService.Invoke(() =>
                        {
                            groupMemberListManager.AddGroupMemberList(gIdx, groupMemberList);
                            string plain = maker + "님이 채팅방을 만드셨습니다.";
                            groupChatManager.addChattingList(gIdx, groupName, plain, time);
                            GroupChattingRoomManager.Instance.makeChatRoom(gIdx, groupName);
                            groupChatManager.addChat(gIdx, new GroupChatItem(plain, maker, time, false));
                            localDAO.GroupInfoCreate(gIdx, groupName, maker + "^" + tokens[5]);
                            localDAO.GroupChattingCreate(maker, gIdx, time, plain);
                        });
                    }
                    string myId = networkManager.MyId;
                    networkManager.SendData("<GMQ>", myId);
                    isit = 3;
                }
                else if (tag.Equals("<GMQ>"))
                {
                    int messageCount = int.Parse(tokens[1]);
                    for (int i = 0; i < messageCount; i++)
                    {
                        string[] groupInfo = tokens[i + 2].Split('^');
                        string sender = groupInfo[0];
                        string gIdx = groupInfo[1];
                        string plain = groupInfo[2];
                        string time = groupInfo[3];
                        string groupName = groupChatManager.getGroupName(gIdx);
                        DispatchService.Invoke(() =>
                        {
                            groupChatManager.addChat(gIdx, new GroupChatItem(plain, sender, time, false));
                            groupChatManager.addChattingList(gIdx, groupName, plain, time);
                            localDAO.GroupChattingCreate(sender, gIdx, time, plain);
                        });
                    }
                    if (!FriendsUserControlViewModel.Instance.loadAllChk)
                    {
                        networkManager.SendData("<FLD>", networkManager.MyId);
                    }
                    FriendsUserControlViewModel.Instance.loadAllChk = true;
                    isit = 3;
                }
                else
                {
                    isit = 2;
                }
                if (networkManager.nowConnect == true) //예외 처리 obj beginreceive
                {
                    if (isit == 1) // 동기
                    {
                        // 처리 안해줘도 된다.
                    }
                    else if (isit == 3) // 동기 + 비동기 합치기
                    {
                        ao.ClearBuffer();

                        // 수신 대기
                        ao.WorkingSocket.Receive(ao.Buffer);
                        string next = Encoding.UTF8.GetString(ao.Buffer);
                        syncReceive(next);
                    }
                    else // 비동기로 전환
                    {
                        ao.ClearBuffer();

                        // 수신 대기
                        ao.WorkingSocket.BeginReceive(ao.Buffer, 0, 4096, 0, sr.DataReceived, ao);
                    }
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
        public SyncSocketReceiver()
        {
            nowSock = networkManager.ProgramSock;
        }
    }
}
