using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
namespace SQLite
{
    class Test
    {
        static void Main(string[] args)
        {
            SQLiteConnection.CreateFile(@"Chatting");
            SQLiteConnection conn = new
                SQLiteConnection(@"Data Source=Chatting;Version=3");
                conn.Open();
                string query = "create table chatting (receiver varchar(20),sender varchar(20))";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                int result = command.ExecuteNonQuery();

                query = "insert into chatting (receiver,sender) values ('안녕하세요 저는 신다민입니다','Damin')";
                command = new SQLiteCommand(query, conn);
                result = command.ExecuteNonQuery();

                query = "select * from chatting";

                command = new SQLiteCommand(query, conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader["sender"].ToString());
                    Console.WriteLine(reader["receiver"].ToString());
                }
                reader.Close();
                conn.Close();
        }

    }
}
