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
namespace Client
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private string str()
        {
            string conn = "Server=localhost; Port=3306; Database=member; Uid=root; Pwd=emforhsqhf1";
            return conn;
        }

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Sign_Click(object sender, RoutedEventArgs e)
        {
            Window w = new w1();
            w.Show();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection damin1 = new MySqlConnection(str());
            string ID1 = id.Text;
            string PASSWORD1 = password.Password;
            string idquery = "SELECT password FROM members WHERE id='" + ID1 + "'";
            damin1.Open();
            MySqlCommand daminid = new MySqlCommand(idquery, damin1);
            MySqlDataReader idreader = daminid.ExecuteReader();
            Application.Current.Properties["id"] = ID1;
            try
            {
                if (idreader.Read())
                {
                    if (PASSWORD1.Equals(idreader["password"] as string))
                    {
                        MessageBox.Show("로그인 성공");
                        Window loginpage = new LoginPage();
                        loginpage.Show();
                        this.Close();
                    }
                    else
                        MessageBox.Show("비밀번호 틀림");
                }
                else
                {
                    MessageBox.Show("아이디,비밀번호를 확인해주세요");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                damin1.Close();
            }
        }
    }
}
