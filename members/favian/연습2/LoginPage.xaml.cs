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
namespace 연습2
{
    /// <summary>
    /// LoginPage.xaml에 대한 상호 작용 논리
    /// </summary>
    /// client 보고 바꿔주기, 친구받았다는거 바로 뜨기, 친구 표시해주기
    /// server를 어떻게 할지 
    /// (ex 지금 여기는 server에서 client를 만들어서 하는데 만약 여기 client가 종료하면...
    public partial class LoginPage : Window //receive도 구현하기
    {
        static string find; 
        static byte[] bb = new byte[100];
        static Socket daminserver;
        static Socket tempsocket;
        string ID = Application.Current.Properties["id"].ToString();

        static void sendm(IAsyncResult a)
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
            MakeServer();
        }

        public  void MakeServer()
        {
            daminserver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            daminserver.Bind(new IPEndPoint(IPAddress.Any, 10801));
            daminserver.Listen(100);
        }
        private void LabelShow()
        {
            MainWindow damin = new MainWindow();
            Binding b = new Binding();
            b.Source = ID + SID.Content;
            SID.SetBinding(Label.ContentProperty, b);
        }

        private void MakeClient()
        {
            tempsocket = daminserver.Accept();
            tempsocket.BeginReceive(bb, 0, bb.Length, SocketFlags.None, new AsyncCallback(receivemsg), tempsocket);
        }

        private void request() 
        {
            MakeClient();
            byte[] msg = Encoding.Default.GetBytes(ID + "님에게 친구 요청이 왔습니다\n 수락하시겠습니까?");
            tempsocket.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(sendm), tempsocket);
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
            catch(Exception ex)
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
                bool re = false;
                Socket dClient = (Socket)a.AsyncState;
                int strlen = dClient.EndReceive(a);
                string r = Encoding.Default.GetString(bb);
                for (int i = strlen - 1; i >= 0; i--)
                {
                    if (r[i] == '?')
                    {
                        strlen = i;
                    }
                    else if (r[i] == '.')
                    {
                        strlen = i;
                    }
                    else if (r[i] == '받')
                    {
                        re = true;
                    }
                }
                if (r[strlen] == '?')
                {
                    if (MessageBoxResult.Yes == MessageBox.Show(r, "친구 요청", MessageBoxButton.YesNo))
                    {
                        byte[] msg = Encoding.Default.GetBytes(ID + "님이 친구 요청을 받았습니다.");
                        tempsocket.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(sendm), tempsocket);
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
                        tempsocket.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(sendm), tempsocket);
                    }
                }
                else
                {
                    MessageBox.Show(r, "친구 요청");
                    if (re)
                    {
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
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                tempsocket.BeginReceive(bb, 0, bb.Length, SocketFlags.None, new AsyncCallback(receivemsg), tempsocket);
            }
        }
    }
}
