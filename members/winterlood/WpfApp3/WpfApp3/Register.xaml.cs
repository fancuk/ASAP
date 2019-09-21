using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
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
    /// Register.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Register : Page
    {
        private string Res { get; set; }
        public Register()
        {
            InitializeComponent();
            this.DataContext = IDpanel;
            Res = "ㅎㅎ";
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
        void onTCid(Object sender, TextChangedEventArgs e)
        {
            if (inputID.Text == "")
            {
                // Create an ImageBrush.
                System.Drawing.Bitmap img = WpfApp3.Properties.Resources.IDwater;
                MemoryStream imgStream = new MemoryStream();
                img.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);
                imgStream.Seek(0, SeekOrigin.Begin);
                BitmapFrame newimg = BitmapFrame.Create(imgStream);
      
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = newimg;
                inputID.Background = ib;
            }
            else
            {
                inputID.Background = null;
            }
          
        }
        void onTCpw(Object sender, TextChangedEventArgs e)
        {
            if (inputPW.Text == "")
            {
                // Create an ImageBrush.
                System.Drawing.Bitmap img = WpfApp3.Properties.Resources.PWwater;
                MemoryStream imgStream = new MemoryStream();
                img.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);
                imgStream.Seek(0, SeekOrigin.Begin);
                BitmapFrame newimg = BitmapFrame.Create(imgStream);

                ImageBrush ib = new ImageBrush();
                ib.ImageSource = newimg;
                ib.AlignmentX = AlignmentX.Left;
                ib.Stretch = Stretch.Uniform;
                inputPW.Background = ib;
            }
            else
            {
                inputPW.Background = null;
            }
        }
        private MySqlConnection getConn()
        {
            string strConn = "Server=localhost; Port=3306; Database=testdb; Uid=root;Pwd=123";
            MySqlConnection conn = new MySqlConnection(strConn);
            return conn;
        }

        void  IDchk(object sender, RoutedEventArgs e)
        {
            if (inputID.Text == "")
            {
                MessageBox.Show("ID를 입력하세요");
                inputID.Focus();
                return;
            }
            MySqlConnection conn = getConn();
            conn.Open();
            string sql1 = "SELECT * FROM members WHERE id='" + inputID.Text + " '";
            MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
            MySqlDataReader read = cmd1.ExecuteReader();
            if (read.Read())
            {
                conn.Close();
                ImageBrush br = new ImageBrush();
                Res = "중복 id입니다.";
                resultIDchk.Text = "중복된 아이디 입니다!";
            }
            else
            {
                conn.Close();
                Res = "사용 가능한 id입니다.";
                resultIDchk.Text = "사용할 수 있는 아이디 입니다!";
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
           if(inputID.Text == null)
            {
                MessageBox.Show("ID를 입력하세요");
                inputID.Focus();
                return;
            }
            if (inputPW.Text == null)
            {
                MessageBox.Show("PW를 입력하세요");
                inputPW.Focus();
                return;
            }
            if (resultIDchk.Text == "사용할 수 있는 아이디 입니다!")
            {
                string sql2 = "INSERT INTO members(id, passwd) VALUES ('" + inputID.Text + "','" + inputPW.Text + "')";
                MySqlConnection conn = getConn();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql2, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("회원가입 성공!");
                NavigationService.Navigate(
     new Uri("Login.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("ID중복검사를 실시해 주세요.");
            }
        }
        void LoginMove(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(
    new Uri("Login.xaml", UriKind.Relative));
        }
        void HyperlikOnRequestNavigate(object sender, RequestNavigateEventArgs args)
        {
            NavigationService.Navigate(args.Uri);
            args.Handled = true;
        }
    }
}
