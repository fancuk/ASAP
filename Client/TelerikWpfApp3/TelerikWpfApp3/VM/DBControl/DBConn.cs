using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;
namespace DBConn
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
            if (!System.IO.File.Exists(db))
            {
                SQLiteConnection.CreateFile("Chatting");
                Conn.Open();
                string Query = "create table Chatting" +
                    " (sender varchar(20),receiver varchar(20),time varchar(20),msg varchar(200))";
                SQLiteCommand command = new SQLiteCommand(Query, Conn);
                int Result = command.ExecuteNonQuery();
            }
                Conn.Close();
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
                "INSERT INTO chatting(sender,receiver,time,msg) " +
                "VALUES('" + Sender + "','" + Receiver + ",'" + Time + ",'" + Msg + "')"; //timestamp,datetime

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
        public ObservableCollection<string> ChattingRead(string FriendID) //채팅 목록
        {
            ObservableCollection<string> information =
                new ObservableCollection<string>();
            SQLiteConnection Conn = new
                SQLiteConnection("Data Source=Chatting;Version=3");
            string query = "Select msg From Chatting Where sender='" + FriendID + "' Order" +
                "By time ASC"; //시간 표시
            try
            {
                Conn.Open();
                SQLiteCommand Command = new SQLiteCommand(query, Conn);
                SQLiteDataReader Datareader = Command.ExecuteReader();
                while (Datareader.Read())
                {
                    string msg = Datareader.ToString();
                    information.Add(msg);
                }
            }
            catch (Exception e)
            {
                //추가
            }
            finally
            {
                Conn.Close();
            }
            return information;
        }
        #endregion
        #region Update 아이디 바꾸기,대화 수정 등등...
        /*
    public bool update(string BeforeId,string AfterId)
    {
        bool flag = false;
        string query = "Update members Set " +
            "id =" + AfterId + " Where id = '" + BeforeId;
        MySqlConnection Conn = new MySqlConnection(str());
        try
        {
            Conn.Open();
            MySqlCommand Command = new MySqlCommand(query, Conn);
            Command.ExecuteNonQuery();
            flag = true;
        }
        catch(Exception e)
        {
            // 추가
        }
        finally
        {
            Conn.Close();
        }
        return flag;
    }
    #endregion
*/
        /*#region Delete 친구삭제, 대화삭제 등등...
        public bool delete(string Id)
        {
            bool flag = false;
            string query = "Delete from members " +
                "Where id ='" + Id + "'";
            MySqlConnection Conn = new MySqlConnection(str());
            try
            {
                Conn.Open();
                MySqlCommand Command = new MySqlCommand(query, Conn);
                Command.ExecuteNonQuery();
                flag = true;
            }
            catch(Exception e)
            {
                //추가
            }
            finally
            {
                Conn.Close();
            }

            return flag;
        }*/
        #endregion
    }
}
