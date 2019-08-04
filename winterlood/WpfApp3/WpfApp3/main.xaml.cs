using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Runtime.InteropServices;
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

namespace WpfApp3
{
    /// <summary>
    /// main.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class main : Window
    {
        public main()
        {
            InitializeComponent();
            if (Application.Current.Properties["id"] != null)
            {
                userid.Text = Application.Current.Properties["id"].ToString();
                userid.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {

            }
          
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Window w = new MainWindow() as NavigationWindow;
            
            Application.Current.Properties["id"] = null;
            w.Show();
            this.Close();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void MaxButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState =
                (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void Frame_FragmentNavigation(object sender, System.Windows.Navigation.FragmentNavigationEventArgs e)
        {

        }
     

    }
}
