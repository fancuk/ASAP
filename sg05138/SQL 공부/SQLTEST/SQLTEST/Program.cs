using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace SQLTEST
{
    class Program
    {
        static void Main(string[] args)
        {
            string db = "jeonrane.db";
            SQLiteConnection conn = new SQLiteConnection(@"Data Source=C:\Users\sg051\source\repos\SQLTEST\jeonrane.db;Version=3;");
            if (!System.IO.File.Exists(db))
            {
                SQLiteConnection.CreateFile(@"C:\Users\sg051\source\repos\SQLTEST\jeonrane.db");
                conn.Open();
                string sql = "create table members (SENDER varchar(20), RECEIVER varchar(20), MSG text)";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                int result = command.ExecuteNonQuery();
                string sqll = "insert into members (SENDER,RECEIVER,MSG) values ('김효빈','이정환','메롱')";
                SQLiteCommand cmd = new SQLiteCommand(sqll, conn);
                int resultt = cmd.ExecuteNonQuery();
                string sqlll = "select * from members";
                SQLiteCommand cmmd = new SQLiteCommand(sqlll, conn);
                SQLiteDataReader read = cmmd.ExecuteReader();
                while (read.Read())
                {
                    Console.WriteLine(read["SENDER"] + " " + read["RECEIVER"] + " " + read["MSG"]);
                }
                read.Close();
            }
            else
            {
                conn.Open();
                string sqll = "insert into members (SENDER,RECEIVER,MSG) values ('김효빈','이정환','메롱')";
                SQLiteCommand cmd = new SQLiteCommand(sqll, conn);
                int resultt = cmd.ExecuteNonQuery();
                string sqlll = "select * from members";
                SQLiteCommand cmmd = new SQLiteCommand(sqlll, conn);
                SQLiteDataReader read = cmmd.ExecuteReader();
                while (read.Read())
                {
                    Console.WriteLine(read["SENDER"] + " " + read["RECEIVER"] + " " + read["MSG"]);
                }
                read.Close();
            }
        }
    }
}
