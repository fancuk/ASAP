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
        GroupMemberListManager groupMemberListManager;
        GroupChatManager groupManager;
        public LocalDAO()
        {
            groupMemberListManager = ((App)Application.Current).groupMemberListManager;
            chatManager = ((App)Application.Current).chatManager;
            groupManager = ((App)Application.Current).groupChatManager;
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
                // Made By 정구
                // 그룹 채팅방 관리를 위한 테이블 생성
                // CREATE 두번 실행하기 위해 트랜잭션 생성

                SQLiteCommand command = Conn.CreateCommand();
                SQLiteTransaction trans;

                trans = Conn.BeginTransaction();
                command.Connection = Conn;
                command.Transaction = trans;

                command.CommandText = "CREATE TABLE IF NOT EXISTS " + myId +
                     " (sender VARCHAR(20), receiver VARCHAR(20), time VARCHAR(20), msg VARCHAR(200), isRead TINYINT)";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS GroupFor" + myId +
                    " (GIDX VARCHAR(10), groupname VARCHAR(30), name VARCHAR(1100))";
                command.ExecuteNonQuery();
                trans.Commit();

                Conn.Close();

                /*string Query = "create table if not exists " + myId +
                     " (sender varchar(20), receiver varchar(20), time varchar(20), msg varchar(200), isRead TINYINT)";
                SQLiteCommand command = new SQLiteCommand(Query, Conn);
                int Result = command.ExecuteNonQuery();
                Conn.Close();*/

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
                    if (status == 1)
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
                bool isGroup = false;
                while (Datareader.Read())
                {
                    string msg = Datareader["msg"].ToString();
                    string sender = Datareader["sender"].ToString();
                    string receiver = Datareader["receiver"].ToString();
                    string time = Datareader["time"].ToString();
                    int status = int.Parse(Datareader["isRead"].ToString()); //true 1 false 0, 1은 읽은거, 0은 안읽은거

                    Chatitem tmpChatItem = new Chatitem();
                    GroupChatItem tmpGroupChatItem = new GroupChatItem();
                    if (status != 3)
                    {
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
                    }
                    else
                    {
                        tmpGroupChatItem.User = sender;
                        tmpGroupChatItem.Text = msg;
                        tmpGroupChatItem.Time = time;

                        // 여기는 그룹 채팅 넣어주는 거. 읽었다고 처리할려고 true로 반환해줌.
                        isGroup = true;
                    }
                    if (isGroup == true) // 그룹채팅때 수행, addChat은 저렇게 넣어야 됨.
                    {
                        if (sender.Equals(myId)) // 그룹 챗 보낸 사람이 나다!
                        {
                            tmpGroupChatItem.Chk = true;
                            // 넣는 순서는 receiver, tmpChatItem
                            // 이때 receiver는 Gidx이다.
                            groupManager.addChat(receiver, tmpGroupChatItem);
                        }
                        else // 그룹 챗 보낸 사람이 나는 아니다!
                        {
                            tmpGroupChatItem.Chk = false;
                            groupManager.addChat(receiver, tmpGroupChatItem);
                        }
                        isGroup = false;
                    }
                    else // 그룹 아니면 일반 채팅
                    {
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
                }
                chatManager.setChattingList();
                groupManager.setChattingList();

                // 여기는 그룹 챗 세팅 부분
                // 여기다 추가
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
            //string query = "UPDATE " + myId + " SET isRead = " + 1 + " WHERE sender = '" + reader + "' OR receiver = '" + reader + "'"; // change status

            // isRead = 0 넣은 이유는 GroupChat을 공유하는데 0과 1을 따로 처리하니까 바꿔버림 그냥 그룹 챗은 3으로 때려박을게.
            string query = "UPDATE " + myId + " SET isRead = " + 1 + " WHERE isRead = " + 0 + " AND (sender = '" + reader + "' OR receiver = '" + reader + "')"; // change status
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

        #region Create_GroupChat
        // Made by 정구
        // 그룹 대화 추가는 table이 myId이여야 하고, isRead를 예외적으로 3으로 처리함으로써 그룹으로 인식하겠끔 사용
        // 3이 되면 싱글 챗과 전혀  관련이 없어진다. 또한 Chat 읽기도 수정해놓음.
        public bool GroupChattingCreate(string Sender, string Gidx, string Time, string Msg) // 그룹 대화 추가
        {
            string myId = networkManager.MyId;
            createChattingFile(myId);
            bool flag = false;
            string query;

            query =
                   "INSERT INTO " + myId + "(sender,receiver,time,msg,isRead) " +
                   "VALUES('" + Sender + "','" + Gidx + "','" + Time + "','" + Msg + "','" + 3 + "') ";
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

        #region Create_GroupList
        // Made By 정구
        // 그룹챗이 생성되면 생성된 그룹방 정보 Insert - GroupIndex 와 UseerName들이 들어감.

        public bool GroupInfoCreate(string GIDX, string GroupName, string GroupUsers) // Insert Group Data
        {
            string myId = networkManager.MyId;
            createChattingFile(myId);
            bool flag = false;
            string query = "INSERT INTO GroupFor" + myId + "(GIDX, groupname, name) " +
                   "VALUES('" + GIDX + "','" + GroupName + "','" + GroupUsers + "') ";


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

        #region Read_GroupList
        // Made By 정구
        // 그룹 채팅방 정보를 읽기 위한 Read Function

        public void ReadGroupList() // Read Group Data
        {
            string myId = networkManager.MyId;
            createChattingFile(myId);
            string query = "SELECT DISTINCT * FROM GroupFor" + myId;

            SQLiteConnection Conn = new
                SQLiteConnection("Data Source=Chatting;Version=3");
            try
            {
                Conn.Open();
                SQLiteCommand Command = new SQLiteCommand(query, Conn);
                Command.ExecuteNonQuery();

                SQLiteDataReader Datareader = Command.ExecuteReader();

                while (Datareader.Read())
                {

                    string GIDX = Datareader["GIDX"].ToString();
                    string groupname = Datareader["groupname"].ToString();
                    string name = Datareader["name"].ToString();
                    List<string> groupMembers = new List<string>();
                    string[] nameSlice = name.Split('^');
                    int length = nameSlice.Length;
                    for (int i = 0; i < length; i++)
                    {
                        groupMembers.Add(nameSlice[i]);
                    }
                    // 이거는 그룹 리스트 넣어줌
                    groupMemberListManager.AddGroupMemberList(GIDX, groupMembers);
                    groupManager.addGroupName(GIDX, groupname);
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
        }
        #endregion
    }
}