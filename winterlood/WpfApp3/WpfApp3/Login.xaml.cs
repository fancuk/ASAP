using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp3
{
    /// <summary>
    /// Login.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Login : Page 
    {
        public Login()
        {
            InitializeComponent();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWnd = Application.Current.MainWindow as NavigationWindow;
            mainWnd.Close();
        }
        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWnd = Application.Current.MainWindow as NavigationWindow;
            mainWnd.WindowState = WindowState.Minimized;
        }
        private void MaxButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWnd = Application.Current.MainWindow as NavigationWindow;
            mainWnd.WindowState = (mainWnd.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }
        private MySqlConnection getConn()
        {
            string strConn = "Server=localhost; Port=3306; Database=testdb; Uid=root;Pwd=123";
            MySqlConnection conn = new MySqlConnection(strConn);
            return conn;
        }
        private Boolean IDchk()
        {
            MySqlConnection conn = getConn();
            conn.Open();
            string sql1 = "SELECT * FROM members WHERE id='" + inputID.Text + " '";
            MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
            MySqlDataReader read = cmd1.ExecuteReader();
            if (read.Read())
            {
                conn.Close();
                return false;
            }
            else
            {
                conn.Close();
                return true;
            }
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (inputID.Text == "")
            {
                MessageBox.Show("ID를 입력하세요");
                inputID.Focus();
                return;
            }
            if (inputPW.Text == "")
            {
                MessageBox.Show("PW를 입력하세요");
                inputPW.Focus();
                return;
            }
            string strConn = "Server=localhost; Port=3306; Database=testdb; Uid=root;Pwd=123";
            MySqlConnection conn = new MySqlConnection(strConn);
            conn.Open();
            string sql = "SELECT * FROM members WHERE id='" + inputID.Text+"'AND passwd='" + inputPW.Text + "'";
            MySqlCommand cmd1 = new MySqlCommand(sql, conn);
            MySqlDataReader read = cmd1.ExecuteReader();
            System.Console.Write("%s\n", inputID.Text);
            System.Console.Write("%s\n", inputPW.Text);
            if (read.Read())
            {      
                MessageBox.Show("로그인 성공");
                Application.Current.Properties["id"] = inputID.Text;

                Window w = new ChatWindow();
                    w.Show();
                    var mainWnd = Application.Current.MainWindow as NavigationWindow;
                if(mainWnd == null)return;
                mainWnd.Close();
            }
            else MessageBox.Show("로그인 실패");
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(
              new Uri("Register.xaml", UriKind.Relative));
        }
        void HyperlikOnRequestNavigate(object sender, RequestNavigateEventArgs args)
        {
            NavigationService.Navigate(args.Uri);
            args.Handled = true;
        }
    }
}
