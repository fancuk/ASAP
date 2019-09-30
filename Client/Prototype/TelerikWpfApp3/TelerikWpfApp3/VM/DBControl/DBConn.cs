using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using TelerikWpfApp3.M;
using System.Windows;

namespace TelerikWpfApp3.VM.DBControl
{
    class DAO
    {
        
    }
    public class database
    {
        public bool chatIsIt = false;
        #region createChattingFile
        private void createChattingFile()
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
                string Query = "create table if not exists Chatting" +
                     " (sender varchar(20),receiver varchar(20),time varchar(20),msg varchar(200))";
                SQLiteCommand command = new SQLiteCommand(Query, Conn);
                int Result = command.ExecuteNonQuery();
                Conn.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        #endregion
        #region Create
        public bool ChattingCreate(string Sender, string Receiver, string Time, string Msg) // 대화 추가
        {
            // 채팅 목록을 그냥 그대로 보여주고(계속 read안하고)
            // 그 채팅 삭제하면, oc에서 그 인덱스만 삭제하거나, 전체 삭제 하면 안되나?
            createChattingFile();
            bool flag = false;
            string query =
                "INSERT INTO Chatting(sender,receiver,time,msg) " +
                "VALUES('" + Sender + "','" + Receiver + "','" + Time + "','" + Msg + "')"; //timestamp,datetime

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
        public ObservableCollection<Chatitem> ChattingRead(string FriendID) //채팅 목록
        {
            createChattingFile();
            ObservableCollection<Chatitem> information =
                new ObservableCollection<Chatitem>();
            SQLiteConnection Conn = new
                SQLiteConnection("Data Source=Chatting;Version=3");
            string query = "select * from Chatting where receiver='" + FriendID + "' order " +
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
                    Chatitem tmpChatItem = new Chatitem();
                    tmpChatItem.User = sender;
                    tmpChatItem.Text = msg;
                    tmpChatItem.Time = time;
                    tmpChatItem.Chk = true;
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
            string myId = ((App)Application.Current).myID;
            createChattingFile();
            ObservableCollection<Chatitem> information =
                new ObservableCollection<Chatitem>();
            SQLiteConnection Conn = new
                SQLiteConnection("Data Source=Chatting;Version=3");
            string query = "select * from Chatting order by time asc"; //시간 표시
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
                    Chatitem tmpChatItem = new Chatitem();
                    tmpChatItem.User = sender;
                    tmpChatItem.Text = msg;
                    tmpChatItem.Time = time;
                    if (sender.Equals(myId))
                    {
                        tmpChatItem.Chk = true;
                        ((App)Application.Current).AddSQLChat(receiver, tmpChatItem);
                    }
                    else
                    {
                        tmpChatItem.Chk = false;
                        ((App)Application.Current).AddSQLChat(sender, tmpChatItem);
                    }                    
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
        }
        #endregion
    }
}
