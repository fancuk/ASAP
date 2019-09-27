using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.M;
using TelerikWpfApp3.MainSocket;

namespace TelerikWpfApp3.MainSocket
{
    public class SocketReceived
    {
        public bool nowListen = false;
        public SocketMaker Msock = new SocketMaker();

        #region DataReceived
        public void DataReceived(IAsyncResult ar)
        {
            SocketConnecter.AsyncObject obj = (SocketConnecter.AsyncObject)ar.AsyncState;

            try
            {
                //int received = obj.WorkingSocket.EndReceive(ar);
                //if (received <= 0)
                //{
                //    obj.WorkingSocket.Close();
                //    return;
                //}

                // UTF8 인코더를 사용하여 바이트 배열을 문자열로 변환한다.
                bool nowConnect = ((App)Application.Current).nowConnect;
                if (nowConnect == false)
                {
                    return;
                }
                string text = Encoding.UTF8.GetString(obj.Buffer);

                // 0x01 기준으로 짜른다.
                // tokens[0] - 보낸 사람 IP
                // tokens[1] - 보낸 메세지
                string[] tokens = text.Split('/');
                string tag = tokens[0];
                if (tokens.Length == 1 && tag != "<FIN>") return;
                if (tag.Equals("<LOG>")) // 로그인
                {
                    string flag = tokens[1];
                    if (flag.Equals("true"))
                    {
                        //Properties.Settings.Default.loginOK = true; // 로그인 성공여부
                        if (Properties.Settings.Default.idSaveCheck == true)
                        {
                            Properties.Settings.Default.loginIdSave = "";
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            Properties.Settings.Default.loginIdSave = ((App)Application.Current).getmyID();
                            Properties.Settings.Default.Save();
                        }
                        DispatchService.Invoke(() =>
                        {
                            ((App)Application.Current).StartMainWindow();
                        });
                        nowListen = true;
                        string myId = ((App)Application.Current).getmyID();

                        Thread.Sleep(10);

                        byte[] bDts = null;
                        string str = "<FLD>" + '/' + myId + '/';

                        bDts = Encoding.UTF8.GetBytes(str);
                        SocketController.mSock.Send(bDts);
                    }
                    else
                    {
                        MessageBox.Show("Login Failed.....TT");
                        Properties.Settings.Default.loginOK = false;
                        ((App)Application.Current).CloseSocket();
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
                    ((App)Application.Current).CloseSocket();
                }
                else if (tag.Equals("<ICF>")) // ID 체크
                {
                    string flag = tokens[1];
                    if (flag.Equals("true"))
                    {
                        MessageBox.Show("ID Check Sucess! in view");
                        ((App)Application.Current).setidchk(true);
                    }
                    else
                    {
                        MessageBox.Show("ID Check Failed.....TT");
                        ((App)Application.Current).setidchk(false);

                    }
                    ((App)Application.Current).CloseSocket();
                }
                else if (tag.Equals("<FRR>"))
                {
                    if (tokens[1] == "true")
                    {
                        string target = tokens[2];
                        DispatchService.Invoke(() =>
                        {
                            ((App)Application.Current).setfriends(target);
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
                else if (tag.Equals("<MSG>")) // 메세지
                {
                    Chatitem tmp = new Chatitem();
                    tmp.User = tokens[1];
                    tmp.Time = tokens[3];
                    tmp.Text = tokens[4];
                    ((App)Application.Current).setchatting(tokens[1], tokens[2], tokens[3], tokens[4]);
                    DispatchService.Invoke(() =>
                    {
                        ((App)Application.Current).AddSQLChat(tmp.User, tmp);
                    });
                    if (!((App)Application.Current).mqState)
                    {
                        ((App)Application.Current).resetSQLChat(tmp.User);
                    }

                }
                else if (tag.Equals("<FIN>"))
                {
                    if (nowConnect == true)
                    {
                        if (nowConnect == true)
                        {
                            ((App)Application.Current).CloseSocket();
                        }
                        Window vt = TelerikWpfApp3.viewtest.Instance;
                        Window sw = TelerikWpfApp3.StartWindow.Instance;
                        vt.Show();
                        sw.Hide();
                    }
                }
                else if (tag.Equals("<FLD>"))
                {
                    ((App)Application.Current).setfriends(tokens[1]);
                }
                // 텍스트박스에 추가해준다.
                // 비동기식으로 작업하기 때문에 폼의 UI 스레드에서 작업을 해줘야 한다.
                // 따라서 대리자를 통해 처리한다.

                //^^^^^^^^
                // 클라이언트에선 데이터를 전달해줄 필요가 없으므로 바로 수신 대기한다.
                // 데이터를 받은 후엔 다시 버퍼를 비워주고 같은 방법으로 수신을 대기한다.
                if (nowConnect == true) //예외 처리 obj beginreceive
                {
                    obj.ClearBuffer();

                    // 수신 대기
                    obj.WorkingSocket.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
                }
            }
            catch (Exception e)
            {
                if (((App)Application.Current).nowConnect == true)
                {
                    Msock.closeSock();
                }
                MessageBox.Show("서버와의 연결 오류!");
                return;
            }
        }
        #endregion
    }
}
