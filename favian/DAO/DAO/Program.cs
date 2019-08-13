using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using DTO;
using System.Collections.ObjectModel;
namespace DAO
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }


    public class database
    {
        private string str()
        {
            string Conn = "Server=localhost; Port=3306; " +
                "Database=member; Uid=root; Pwd=1234";
            return Conn;
        }
        #region Create
        public bool create(string Name,string Password,string Age)
        {
            bool flag = false;
            string query =
                "INSERT INTO members(id,password,age) " +
                "VALUES('" + Name + "','" + Password + "','" + Age + "')";

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
                
            }
            finally
            {
                Conn.Close();
            }
            return flag;
        }
        #endregion
        #region Read 반환형 ObservableCollection 성공 실패는 반환값을 count
        public ObservableCollection<info> read()
        {
            ObservableCollection<info> information = new ObservableCollection<info>();
            MySqlConnection Conn = new MySqlConnection(str());
            string query = "SELECT * FROM members " +
                    "ORDER BY id ASC";
            try
            {
                Conn.Open();
                MySqlCommand Command = new MySqlCommand(query, Conn);
                MySqlDataReader Datareader = Command.ExecuteReader();
                while (Datareader.Read())
                {
                    string Id = Datareader["id"].ToString();
                    string Password = Datareader["password"].ToString();
                    string Age = Datareader["Age"].ToString();
                    info member = new info(Id, Password, Age);
                    information.Add(member);
                }
            }
            catch(Exception e)
            {

            }
            finally
            {
                Conn.Close();
            }
            return information;
        }
        #endregion
        #region Update 2개 인자로 받아서 바꿀아이디, 바뀔아이디
        public bool update(string BeforeId,string AfterId)
        {
            bool flag = false;
            string query = "Update members Set " +
                "id =" + AfterId + " Where id = '" + BeforeId;
            return flag;
        }
        #endregion
        #region Delete 1개 인자로 받아서 삭제할 아이디
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

            }
            finally
            {
                Conn.Close();
            }

            return flag;
        }
        #endregion
    }
}
