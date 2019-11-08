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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Activities;
using System.Drawing;


namespace TelerikWpfApp3.View.Alert
{
    /// <summary>
    /// ServerError.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MSGAlert : Window
    {
        public MSGAlert()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 창의위치를 셋팅 (우측하단에 올라왔다 내려가게 할 예정)
            this.Left = SystemParameters.PrimaryScreenWidth -100;
            this.Top = SystemParameters.PrimaryScreenHeight - 60;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
