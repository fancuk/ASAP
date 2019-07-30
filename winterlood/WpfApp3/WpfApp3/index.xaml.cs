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
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace WpfApp3
{
    /// <summary>
    /// index.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class index : Page 
    {
        public index()
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
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(
                new Uri("Login.xaml", UriKind.Relative));
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
