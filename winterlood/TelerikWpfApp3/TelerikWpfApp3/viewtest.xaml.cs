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
namespace TelerikWpfApp3
{
    /// <summary>
    /// viewtest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class viewtest : Window
    {
        public viewtest()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += MoveWindow;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
        void MoveWindow(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }
        public void Login(object sender, RoutedEventArgs e)
        {
            string Uid = idbox.Text;
            string Upw = pwbox.Password.ToString();
            ((App)Application.Current).setmyID(Uid);
            string parameter = Uid + "/" + Upw + "/";
            ((App)Application.Current).SendData("<LOG>",parameter);
        }

    }
}
