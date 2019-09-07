using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.VM.DBControl
{
    class DBConn
    {
        public DBConn()
        {
           
        }
        public MySqlConnection getConn()
        {
            string strConn = "Server=localhost; Port=3306; Database=testdb; Uid=root;Pwd=123";
            MySqlConnection conn = new MySqlConnection(strConn);
            return conn;
        }
    }
}
