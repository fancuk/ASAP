using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Net.Sockets;
using System.Net;
using System.Windows.Threading;

namespace Client
{
    /// <summary>
    /// LoginPage.xaml에 대한 상호 작용 논리
    /// </summary>
    /// socket close 구현 껐다가 켜도 친구 있어야함 이미 친구인 사람 친구 추가 방지
    public partial class LoginPage : Window
    {
        static string find;
        static byte[] bb = new byte[100];
        static Socket daminClient;
        string ID = Application.Current.Properties["id"].ToString();

        private void MakeClient()
        {
            daminClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            daminClient.Connect(new IPEndPoint(IPAddress.Loopback, 10801));
            daminClient.BeginReceive(bb, 0, bb.Length, SocketFlags.None, new AsyncCallback(receivemsg), daminClient);
        }

        private void sendm(IAsyncResult a)
        {
            Socket clientsocket = (Socket)a.AsyncState;
            int mlen = clientsocket.EndSend(a);
        }
        private string getstr()
        {
            string conn = "Server=localhost; Port=3306; Database=member; Uid=root; Pwd=emforhsqhf1";
            return conn;
        }
        public LoginPage()
        {
            InitializeComponent();
            LabelShow();
            MakeClient();
        }

        private void LabelShow()
        {
            MainWindow damin = new MainWindow();
            Binding b = new Binding();
            b.Source = ID + SID.Content;
            SID.SetBinding(Label.ContentProperty, b);
        }

        private void request()
        {
            byte[] msg = Encoding.Default.GetBytes(ID + "님에게 친구 요청이 왔습니다\n 수락하시겠습니까?");
            try
            {
                daminClient.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(sendm), daminClient);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void FID_TextChanged(object sender, TextChangedEventArgs e)
        {
            find = FID.Text;
        }

        private void FPlus_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection daminconn = new MySqlConnection(getstr());
            string idquery = "SELECT id FROM members WHERE id='" + find + "'";
            if (find == ID)
            {
                MessageBox.Show("장난치지마십쇼!");
                return;
            }
            try
            {
                daminconn.Open();
                MySqlCommand damincommand = new MySqlCommand(idquery, daminconn);
                MySqlDataReader daminread = damincommand.ExecuteReader();
                if (daminread.Read())
                {
                    MessageBox.Show("친구 요청을 보냈습니다.");
                    request();
                }
                else
                {
                    MessageBox.Show("해당 ID는 존재하지 않습니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                daminconn.Close();
            }
        }

        private void receivemsg(IAsyncResult a)
        {
            try
            {
                Socket dClient = (Socket)a.AsyncState;
                int strlen = dClient.EndReceive(a);
                string r = Encoding.Default.GetString(bb);
                for(int i = strlen - 1; i >= 0; i--)
                {
                    if (r[i] == '?')
                    {
                        strlen = i;
                        break;
                    }
                    else if (r[i] == '.')
                    {
                        strlen = i;
                        break;
                    }
                }
                if (r[strlen] == '?')
                {
                    if (MessageBoxResult.Yes == MessageBox.Show(r, "친구 요청", MessageBoxButton.YesNo))
                    {
                        byte[] msg = Encoding.Default.GetBytes(ID + "님이 친구 요청을 받았습니다.");
                        daminClient.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(sendm), daminClient);
                        string Friend = null;
                        for (int i = 0; i < r.Length; i++)
                        {
                            if (r[i] == '님') break;
                            Friend += r[i];
                        }
                        Binding b = new Binding();
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                        {
                            b.Source = Friend + "\n" + FName.Content;
                            FName.SetBinding(Label.ContentProperty, b);
                        }));
                    }
                    else
                    {
                        byte[] msg = Encoding.Default.GetBytes(ID + "님이 친구 요청을 거절하였습니다.");
                        daminClient.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(sendm), daminClient);
                    }
                }
                else
                {
                    MessageBox.Show(r, "친구 요청");

                    string Friend = null;
                    for (int i = 0; i < r.Length; i++)
                    {
                        if (r[i] == '님') break;
                        Friend += r[i];
                    }
                    Binding b = new Binding();
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        b.Source = Friend + "\n" + FName.Content;
                        FName.SetBinding(Label.ContentProperty, b);
                    }));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                daminClient.BeginReceive(bb, 0, bb.Length, SocketFlags.None, new AsyncCallback(receivemsg), daminClient);
            }
        }
    }
}
