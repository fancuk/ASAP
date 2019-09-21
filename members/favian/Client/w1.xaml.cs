using MySql.Data.MySqlClient;
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
using MySql.Data;
namespace Client
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class w1 : Window
    {
        public w1()
        {
            InitializeComponent();
        }

        bool dataflag = false;
        private string str()
        {
            string conn1 = "Server=localhost; Port=3306; Database=member; Uid=root; Pwd=emforhsqhf1";
            return conn1;
        }


        private void Already_Click(object sender, RoutedEventArgs e)
        {
            string ID = IDBOX.Text;
            string query = "Select id FROM members WHERE id='" + ID + "'";
            MySqlConnection damin = new MySqlConnection(str());
            damin.Open();
            MySqlCommand damincommand = new MySqlCommand(query, damin);
            MySqlDataReader dataReader = damincommand.ExecuteReader();
            try
            {
                if (!dataReader.Read())
                {
                    MessageBox.Show("사용 가능");
                    dataflag = true;
                }
                else
                {
                    MessageBox.Show("사용 불가");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                damin.Close();
            }
        }

        private void Complete_Click(object sender, RoutedEventArgs e)
        {
            string ID2 = IDBOX.Text;
            string PASSWORD = PASSWORDBOX.Text;
            string age = AGEBOX.Text;
            int realage = Int32.Parse(age);
            string query1 = "INSERT INTO members(id,password,age) VALUES('" + ID2 + "','" + PASSWORD + "','" + realage + "')";
            MySqlConnection damin2 = new MySqlConnection(str());

            if (ID2 == "" || PASSWORD == "" || age == "")
            {
                MessageBox.Show("아이디, 비밀번호, 나이는 빈칸일 수 없습니다.");
            }
            else
            {
                damin2.Open();
                try
                {
                    if (dataflag)
                    {
                        MySqlCommand damincommand1 = new MySqlCommand(query1, damin2);
                        int res = damincommand1.ExecuteNonQuery();
                        MessageBox.Show("회원 가입 완료", "성공");
                    }
                    else
                    {
                        MessageBox.Show("중복 확인 해주세요", "실패");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    damin2.Close();
                }
            }
        }



        private void IDBOX_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataflag = false;
        }
    }
}
