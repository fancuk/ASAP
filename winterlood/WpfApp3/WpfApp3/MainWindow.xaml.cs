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
namespace WpfApp3
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {

            InitializeComponent();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(MainWindow_MouseLeftButtonDown);
        }
    
        public  void CloseButton_Click()
        {
            this.Close();
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        public void MinButton_Click()
        {
            this.WindowState = WindowState.Minimized;
        }
            public void MaxButton_Click()
        {
            this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
