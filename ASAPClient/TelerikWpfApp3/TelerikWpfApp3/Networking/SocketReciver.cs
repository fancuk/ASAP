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
        private Socket nowSock;
        SocketCloser sc = new SocketCloser();
        public SocketReciver()
        {
            nowSock = networkManager.ProgramSock;
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
                if (tag.Equals("<LOG>")) // 로그인
                {
                    string flag = tokens[1];
                    if (flag.Equals("true"))
                    {
                        Properties.Settings.Default.loginOK = true; // 로그인 성공여부
                        if (Properties.Settings.Default.idSaveCheck == false)
                        {
                            Properties.Settings.Default.loginIdSave =networkManager.MyId; //id 저장
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
                        string myId =networkManager.MyId;

                        Thread.Sleep(10);
                        if (!FriendsUserControlViewModel.Instance.loadAllChk)
                        {
                           networkManager.SendData("<FLD>",networkManager.MyId);
                        }
                        FriendsUserControlViewModel.Instance.loadAllChk = true; //다민
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
                else if (tag.Equals("<FRR>"))
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

                    // 여기 이제 수정 필요!!
                   localDAO.ChattingCreate(tokens[1], tokens[2], tokens[3], tokens[4], "Receive");
                    DispatchService.Invoke(() =>
                    {
                       chatManager.addChat(tmp.User, tmp);
                        Window msgWindow = new MSGAlert();
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

                       localDAO.ChattingCreate(user,
                           networkManager.MyId, time, msg, "Receive");
                    }
                }
                else if (tag.Equals("<FIN>"))
                {
                    if (networkManager.nowConnect == true)
                    {
                        sc.closeSock();
                    }
                }
                /*else if (tag.Equals("<FLD>"))
                {

                    int count = tokens.Length;
                    for (int i = 2; i < count; i++)
                    {
                        string[] parsing = tokens[i].Split('^');
                        DispatchService.Invoke(() =>
                        {
                            string friendId = parsing[0];
                            string friendStatus = parsing[1];   
                            ((App)Application.Current).AddFriend(friendId,friendStatus);
                        });
                    }
                }*/
                else if (tag.Equals("CHR")) // 서버 업데이트 후에
                {
                    /*string friendID = tokens[1];
                    string isitFocus = tokens[2];
                    if(isitFocus == "true")
                    {
                        chatManager.AddFriendsReading(friendID);
                    }
                    else
                    {
                        chatManager.RemoveFriendsReading(friendID);
                    }*/
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
                    }
                    DispatchService.Invoke(() =>
                    {
                        localDAO.ReadChat();
                    });
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
