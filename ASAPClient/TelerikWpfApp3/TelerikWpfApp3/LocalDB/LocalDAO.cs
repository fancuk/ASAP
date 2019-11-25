using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using TelerikWpfApp3.M;
using System.Windows;
using TelerikWpfApp3.Service;

namespace TelerikWpfApp3.LocalDB
{
    public class LocalDAO
    {
        NetworkManager networkManager = ((App)Application.Current).networkManager;
        ChatManager chatManager;
        public LocalDAO()
        {
            chatManager = ((App)Application.Current).chatManager;
        }
        public bool chatIsIt = false;
        #region createChattingFile
        private void createChattingFile(string myId)
        {
            string db = @"Chatting";
            SQLiteConnection Conn = new
                    SQLiteConnection("Data Source=Chatting;Version=3");
            Conn.Open();
            if (!System.IO.File.Exists(db))
            {
                SQLiteConnection.CreateFile("Chatting");
            }
            try
            {
                string Query = "create table if not exists " + myId +
                     " (sender varchar(20), receiver varchar(20), time varchar(20), msg varchar(200), isRead TINYINT)";
                SQLiteCommand command = new SQLiteCommand(Query, Conn);
                int Result = command.ExecuteNonQuery();
                Conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        #endregion
        #region Create
        public bool ChattingCreate(string Sender, string Receiver, string Time, string Msg, string type, int Status) // 대화 추가
        {
            // 채팅 목록을 그냥 그대로 보여주고(계속 read안하고)
            // 그 채팅 삭제하면, oc에서 그 인덱스만 삭제하거나, 전체 삭제 하면 안되나?
            createChattingFile(Sender);
            bool flag = false;
            string query;
            if (type == "Send")
            {
                query =
                   "INSERT INTO " + Sender + "(sender,receiver,time,msg,isRead) " +
                   "VALUES('" + Sender + "','" + Receiver + "','" + Time + "','" + Msg + "','" + Status + "') ";
                //"WHERE NOT EXISTS (SELECT Sender, Receiver, Time, Msg FROM Chatting WHERE " +
                //"sender = '"+Sender+"', receiver ='"+Receiver+"', time = '"+Time+"', msg ='"+Msg+"')"; //timestamp,datetime
            }
            else
            {
                query =
                   "INSERT INTO " + Receiver + "(sender,receiver,time,msg,isRead) " +
                   "VALUES('" + Sender + "','" + Receiver + "','" + Time + "','" + Msg + "','" + Status + "') ";
            }
            SQLiteConnection Conn = new
                SQLiteConnection("Data Source=Chatting;Version=3");
            try
            {
                Conn.Open();
                SQLiteCommand Command = new SQLiteCommand(query, Conn);
                Command.ExecuteNonQuery();
                flag = true;
            }
            catch (Exception e)
            {
                //추가
            }
            finally
            {
                Conn.Close();
            }
            return flag;
        }
        #endregion
        #region Read 반환형 ObservableCollection
        public ObservableCollection<Chatitem> ChattingRead(string myId, string FriendID) //채팅 목록
        {
            createChattingFile(myId);
            ObservableCollection<Chatitem> information =
                new ObservableCollection<Chatitem>();
            SQLiteConnection Conn = new
                SQLiteConnection("Data Source=Chatting;Version=3");
            string query = "select distinct * from " + myId + " where receiver='" + myId + "' order " +
                "by time asc"; //시간 표시
            try
            {
                Conn.Open();
                SQLiteCommand Command = new SQLiteCommand(query, Conn);
                SQLiteDataReader Datareader = Command.ExecuteReader();
                while (Datareader.Read())
                {
                    string msg = Datareader["msg"].ToString();
                    string sender = Datareader["sender"].ToString();
                    string receiver = Datareader["receiver"].ToString();
                    string time = Datareader["time"].ToString();
                    int status = int.Parse(Datareader["isRead"].ToString()); //true 1 false 0, 1은 읽은거, 0은 안읽은거
                    Chatitem tmpChatItem = new Chatitem();
                    tmpChatItem.User = sender;
                    tmpChatItem.Text = msg;
                    tmpChatItem.Time = time;
                    tmpChatItem.Chk = true;
                    if(status == 1)
                    {
                        tmpChatItem.Status = true;
                    }
                    else
                    {
                        tmpChatItem.Status = false;
                    }
                    information.Add(tmpChatItem);
                }
            }
            catch (Exception e)
            {
                //추가
                MessageBox.Show(e.ToString());
            }
            finally
            {
                Conn.Close();
            }
            return information;
        }
        #endregion


        #region Read 반환형 ObservableCollection
        public void ReadChat() //채팅 목록
        {
            string myId = networkManager.MyId;
            createChattingFile(myId);
            ObservableCollection<Chatitem> information =
                new ObservableCollection<Chatitem>();
            SQLiteConnection Conn = new
                SQLiteConnection("Data Source=Chatting;Version=3");
            string query = "select  distinct * from " + myId + " order by time asc"; //시간 표시
            try
            {
                Conn.Open();
                SQLiteCommand Command = new SQLiteCommand(query, Conn);
                SQLiteDataReader Datareader = Command.ExecuteReader();
                while (Datareader.Read())
                {
                    string msg = Datareader["msg"].ToString();
                    string sender = Datareader["sender"].ToString();
                    string receiver = Datareader["receiver"].ToString();
                    string time = Datareader["time"].ToString();
                    int status = int.Parse(Datareader["isRead"].ToString()); //true 1 false 0, 1은 읽은거, 0은 안읽은거
                    Chatitem tmpChatItem = new Chatitem();
                    tmpChatItem.User = sender;
                    tmpChatItem.Text = msg;
                    tmpChatItem.Time = time;
                    if (status == 1)
                    {
                        tmpChatItem.Status = true;
                    }
                    else
                    {
                        tmpChatItem.Status = false;
                    }
                    if (sender.Equals(myId))
                    {
                        tmpChatItem.Chk = true;
                        chatManager.addChat(receiver, tmpChatItem);
                    }
                    else if (receiver.Equals(myId))
                    {
                        tmpChatItem.Chk = false;
                        chatManager.addChat(sender, tmpChatItem);
                    }
                }
                chatManager.setChattingList();
            }
            catch (Exception e)
            {
                //추가

                MessageBox.Show(e.ToString());
            }
            finally
            {
                Conn.Close();
            }
        }
        #endregion


        #region Chat status 변경(읽었을 경우)
        public void ChangeChatStatus(string reader) //상대방이 채팅 읽었을 때 1 안읽었을 때 0인데 0으로 처리
        {
            string myId = networkManager.MyId;
            createChattingFile(myId);
            ObservableCollection<Chatitem> information =
                new ObservableCollection<Chatitem>();
            SQLiteConnection Conn = new
                SQLiteConnection("Data Source=Chatting;Version=3");
            //string query = "UPDATE " + myId + " SET isRead = " + 1 + " WHERE isRead = 0 AND (sender = " + reader + " OR receiver = " + reader + " )"; // change status
            string query = "UPDATE " + myId + " SET isRead = " + 1 + " WHERE sender = '" + reader + "' OR receiver = '" + reader + "'"; // change status

            try
            {
                Conn.Open();
                SQLiteCommand Command = new SQLiteCommand(query, Conn);
                Command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                Conn.Close();
            }

        }
        #endregion
    }
}