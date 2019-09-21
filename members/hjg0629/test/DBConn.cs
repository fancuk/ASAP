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
using System.Windows.Navigation;
using System.Windows.Shapes;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;


namespace test
{
    class DBConn
    {
        static string strCon = "SERVER = localhost; DATABASE = group; UID = root; PWD = 1234";
        public MySqlConnection myconn = new MySqlConnection(strCon);
        public void DBOpen()
        {

            try
            {
                myconn.Open();
                MessageBox.Show("DB 연결 완료");
            }
            catch (MySqlException e)
            {
                myconn.Close();
                MessageBox.Show("DB 연결 실패" + " (" + e.Message + ")");
            }
        }
    }
}
